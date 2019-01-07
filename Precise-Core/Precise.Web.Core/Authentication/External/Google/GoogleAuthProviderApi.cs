using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Google;
using Newtonsoft.Json.Linq;

namespace Precise.Web.Authentication.External.Google
{
	// Token: 0x02000011 RID: 17
	public class GoogleAuthProviderApi : ExternalAuthProviderApiBase
	{
		// Token: 0x0600003B RID: 59 RVA: 0x00002520 File Offset: 0x00000720
		public override async Task<ExternalAuthUserInfo> GetUserInfo(string accessCode)
		{
			ExternalAuthUserInfo result;
			using (HttpClient client = new HttpClient())
			{
				client.DefaultRequestHeaders.UserAgent.ParseAdd("Microsoft ASP.NET Core OAuth middleware");
				client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
				client.Timeout = TimeSpan.FromSeconds(30.0);
				client.MaxResponseContentBufferSize = 10485760L;
				HttpResponseMessage httpResponseMessage = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, GoogleDefaults.UserInformationEndpoint)
				{
					Headers = 
					{
						Authorization = new AuthenticationHeaderValue("Bearer", accessCode)
					}
				});
				httpResponseMessage.EnsureSuccessStatusCode();
				JObject user = JObject.Parse(await httpResponseMessage.Content.ReadAsStringAsync());
				result = new ExternalAuthUserInfo
				{
					Name = GoogleHelper.GetName(user),
					EmailAddress = GoogleHelper.GetEmail(user),
					Surname = GoogleHelper.GetFamilyName(user),
					ProviderKey = GoogleHelper.GetId(user),
					Provider = "Google"
				};
			}
			return result;
		}

		// Token: 0x04000012 RID: 18
		public const string Name = "Google";
	}
}
