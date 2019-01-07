using System;
using System.Globalization;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Abp.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;

namespace Precise.Web.Authentication.External.Facebook
{
	// Token: 0x02000013 RID: 19
	public class FacebookAuthProviderApi : ExternalAuthProviderApiBase
	{
		// Token: 0x06000045 RID: 69 RVA: 0x000026B4 File Offset: 0x000008B4
		public override async Task<ExternalAuthUserInfo> GetUserInfo(string accessCode)
		{
			string text = QueryHelpers.AddQueryString("https://graph.facebook.com/v2.8/me", "access_token", accessCode);
			text = QueryHelpers.AddQueryString(text, "appsecret_proof", this.GenerateAppSecretProof(accessCode));
			text = QueryHelpers.AddQueryString(text, "fields", "email,last_name,first_name,middle_name");
			ExternalAuthUserInfo result;
			using (HttpClient client = new HttpClient())
			{
				client.DefaultRequestHeaders.UserAgent.ParseAdd("Microsoft ASP.NET Core OAuth middleware");
				client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
				client.DefaultRequestHeaders.Host = "graph.facebook.com";
				client.Timeout = TimeSpan.FromSeconds(30.0);
				client.MaxResponseContentBufferSize = 10485760L;
				HttpResponseMessage httpResponseMessage = await client.GetAsync(text);
				httpResponseMessage.EnsureSuccessStatusCode();
				JObject user = JObject.Parse(await httpResponseMessage.Content.ReadAsStringAsync());
				string text2 = FacebookHelper.GetFirstName(user);
				string middleName = FacebookHelper.GetMiddleName(user);
				if (!StringExtensions.IsNullOrEmpty(middleName))
				{
					text2 += middleName;
				}
				result = new ExternalAuthUserInfo
				{
					Name = text2,
					EmailAddress = FacebookHelper.GetEmail(user),
					Surname = FacebookHelper.GetLastName(user),
					Provider = "Facebook",
					ProviderKey = FacebookHelper.GetId(user)
				};
			}
			return result;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002704 File Offset: 0x00000904
		private string GenerateAppSecretProof(string accessToken)
		{
			string result;
			using (HMACSHA256 hmacsha = new HMACSHA256(Encoding.ASCII.GetBytes(base.ProviderInfo.ClientSecret)))
			{
				byte[] array = hmacsha.ComputeHash(Encoding.ASCII.GetBytes(accessToken));
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < array.Length; i++)
				{
					stringBuilder.Append(array[i].ToString("x2", CultureInfo.InvariantCulture));
				}
				result = stringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x04000013 RID: 19
		public const string Name = "Facebook";
	}
}
