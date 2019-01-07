using System.Threading.Tasks;
using Abp.Dependency;
using Abp.UI;
using Microsoft.AspNetCore.Identity;
using Precise.Authorization.Users;

namespace Precise.Authentication.TwoFactor.Google
{
    public class GoogleAuthenticatorProvider : PreciseServiceBase, IUserTwoFactorTokenProvider<User>, ITransientDependency
    {
        private readonly GoogleTwoFactorAuthenticateService _googleTwoFactorAuthenticateService;

        public GoogleAuthenticatorProvider(GoogleTwoFactorAuthenticateService googleTwoFactorAuthenticateService)
        {
            _googleTwoFactorAuthenticateService = googleTwoFactorAuthenticateService;
        }

        public const string Name = "GoogleAuthenticator";

        public Task<string> GenerateAsync(string purpose, UserManager<User> userManager, User user)
        {
            CheckIfGoogleAuthenticatorIsEnabled(user);

            var setupInfo = _googleTwoFactorAuthenticateService.GenerateSetupCode("Precise", user.EmailAddress, user.GoogleAuthenticatorKey, 300, 300);

            return Task.FromResult(setupInfo.QrCodeSetupImageUrl);
        }

        public Task<bool> ValidateAsync(string purpose, string token, UserManager<User> userManager, User user)
        {
            CheckIfGoogleAuthenticatorIsEnabled(user);

            return Task.FromResult(_googleTwoFactorAuthenticateService.ValidateTwoFactorPin(user.GoogleAuthenticatorKey, token));
        }

        private void CheckIfGoogleAuthenticatorIsEnabled(User user)
        {
            if (user.GoogleAuthenticatorKey == null)
            {
                throw new UserFriendlyException(L("GoogleAuthenticatorIsNotEnabled"));
            }
        }

        public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<User> userManager, User user)
        {
            return Task.FromResult(user.IsTwoFactorEnabled && user.GoogleAuthenticatorKey != null);
        }
    }
}