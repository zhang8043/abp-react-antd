using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Abp.Dependency;

namespace Precise.Authentication.TwoFactor.Google
{
    /// <summary>
    /// This code taken from https://github.com/BrandonPotter/GoogleAuthenticator
    /// </summary>
    public class GoogleTwoFactorAuthenticateService : ITransientDependency
    {
        public static DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public TimeSpan DefaultClockDriftTolerance { get; set; }
        public bool UseManagedSha1Algorithm { get; set; }
        public bool TryUnmanagedAlgorithmOnFailure { get; set; }
        public GoogleTwoFactorAuthenticateService() : this(true, true) { }

        public GoogleTwoFactorAuthenticateService(bool useManagedSha1, bool useUnmanagedOnFail)
        {
            DefaultClockDriftTolerance = TimeSpan.FromMinutes(5);
            UseManagedSha1Algorithm = useManagedSha1;
            TryUnmanagedAlgorithmOnFailure = useUnmanagedOnFail;
        }

        public GoogleAuthenticatorSetupCode GenerateSetupCode(string accountTitleNoSpaces, string accountSecretKey, int qrCodeWidth, int qrCodeHeight)
        {
            return GenerateSetupCode(null, accountTitleNoSpaces, accountSecretKey, qrCodeWidth, qrCodeHeight);
        }

        public GoogleAuthenticatorSetupCode GenerateSetupCode(string issuer, string accountTitleNoSpaces, string accountSecretKey, int qrCodeWidth, int qrCodeHeight)
        {
            return GenerateSetupCode(issuer, accountTitleNoSpaces, accountSecretKey, qrCodeWidth, qrCodeHeight, true);
        }

        public GoogleAuthenticatorSetupCode GenerateSetupCode(string issuer, string accountTitleNoSpaces, string accountSecretKey, int qrCodeWidth, int qrCodeHeight, bool useHttps)
        {
            accountTitleNoSpaces = accountTitleNoSpaces?.Replace(" ", "") ?? throw new NullReferenceException("Account Title is null");

            var setupCode = new GoogleAuthenticatorSetupCode
            {
                Account = accountTitleNoSpaces,
                AccountSecretKey = accountSecretKey
            };

            var encodedSecretKey = EncodeAccountSecretKey(accountSecretKey);
            setupCode.ManualEntryKey = encodedSecretKey;

            string provisionUrl = UrlEncode(string.IsNullOrEmpty(issuer) ?
                $"otpauth://totp/{accountTitleNoSpaces}?secret={encodedSecretKey}" :
                $"otpauth://totp/{accountTitleNoSpaces}?secret={encodedSecretKey}&issuer={UrlEncode(issuer)}");

            var protocol = useHttps ? "https" : "http";
            var url =
                $"{protocol}://chart.googleapis.com/chart?cht=qr&chs={qrCodeWidth}x{qrCodeHeight}&chl={provisionUrl}";

            setupCode.QrCodeSetupImageUrl = url;

            return setupCode;
        }

        private string UrlEncode(string value)
        {
            var stringBuilder = new StringBuilder();
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

            foreach (var symbol in value)
            {
                if (validChars.IndexOf(symbol) != -1)
                {
                    stringBuilder.Append(symbol);
                }
                else
                {
                    stringBuilder.Append('%' + $"{(int)symbol:X2}");
                }
            }

            return stringBuilder.ToString().Replace(" ", "%20");
        }

        private string EncodeAccountSecretKey(string accountSecretKey)
        {
            return Base32Encode(Encoding.UTF8.GetBytes(accountSecretKey));
        }

        private string Base32Encode(byte[] data)
        {
            var inByteSize = 8;
            var outByteSize = 5;
            var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567".ToCharArray();

            int i = 0, index = 0;
            var result = new StringBuilder((data.Length + 7) * inByteSize / outByteSize);

            while (i < data.Length)
            {
                var currentByte = data[i] >= 0 ? data[i] : (data[i] + 256);

                int digit;
                if (index > inByteSize - outByteSize)
                {
                    int nextByte;
                    if (i + 1 < data.Length)
                        nextByte = (data[i + 1] >= 0) ? data[i + 1] : (data[i + 1] + 256);
                    else
                        nextByte = 0;

                    digit = currentByte & (0xFF >> index);
                    index = (index + outByteSize) % inByteSize;
                    digit <<= index;
                    digit |= nextByte >> (inByteSize - index);
                    i++;
                }
                else
                {
                    digit = (currentByte >> (inByteSize - (index + outByteSize))) & 0x1F;
                    index = (index + outByteSize) % inByteSize;
                    if (index == 0)
                        i++;
                }
                result.Append(alphabet[digit]);
            }

            return result.ToString();
        }

        public string GeneratePinAtInterval(string accountSecretKey, long counter, int digits = 6)
        {
            return GenerateHashedCode(accountSecretKey, counter, digits);
        }

        internal string GenerateHashedCode(string secret, long iterationNumber, int digits = 6)
        {
            var key = Encoding.UTF8.GetBytes(secret);
            return GenerateHashedCode(key, iterationNumber, digits);
        }

        internal string GenerateHashedCode(byte[] key, long iterationNumber, int digits = 6)
        {
            var counter = BitConverter.GetBytes(iterationNumber);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(counter);
            }

            var hmac = GetHmacSha1Algorithm(key);

            var hash = hmac.ComputeHash(counter);

            var offset = hash[hash.Length - 1] & 0xf;

            // Convert the 4 bytes into an integer, ignoring the sign.
            var binary =
                ((hash[offset] & 0x7f) << 24)
                | (hash[offset + 1] << 16)
                | (hash[offset + 2] << 8)
                | (hash[offset + 3]);

            var password = binary % (int)Math.Pow(10, digits);

            return password.ToString(new string('0', digits));
        }

        private long GetCurrentCounter()
        {
            return GetCurrentCounter(DateTime.UtcNow, Epoch, 30);
        }

        private long GetCurrentCounter(DateTime now, DateTime epoch, int timeStep)
        {
            return (long)(now - epoch).TotalSeconds / timeStep;
        }

        private HMACSHA1 GetHmacSha1Algorithm(byte[] key)
        {
            return new HMACSHA1(key);
        }

        public bool ValidateTwoFactorPin(string accountSecretKey, string twoFactorCodeFromClient)
        {
            return ValidateTwoFactorPin(accountSecretKey, twoFactorCodeFromClient, DefaultClockDriftTolerance);
        }

        public bool ValidateTwoFactorPin(string accountSecretKey, string twoFactorCodeFromClient, TimeSpan timeTolerance)
        {
            var codes = GetCurrentPins(accountSecretKey, timeTolerance);
            return codes.Any(c => c == twoFactorCodeFromClient);
        }

        public string[] GetCurrentPins(string accountSecretKey, TimeSpan timeTolerance)
        {
            var codes = new List<string>();
            var iterationCounter = GetCurrentCounter();
            var iterationOffset = 0;

            if (timeTolerance.TotalSeconds > 30)
            {
                iterationOffset = Convert.ToInt32(timeTolerance.TotalSeconds / 30.00);
            }

            var iterationStart = iterationCounter - iterationOffset;
            var iterationEnd = iterationCounter + iterationOffset;

            for (var counter = iterationStart; counter <= iterationEnd; counter++)
            {
                codes.Add(GeneratePinAtInterval(accountSecretKey, counter));
            }

            return codes.ToArray();
        }
    }
}