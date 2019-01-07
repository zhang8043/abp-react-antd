using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Newtonsoft.Json.Linq;

namespace Precise.Web.Authentication.External.Microsoft
{
	// Token: 0x02000010 RID: 16
	public class MicrosoftAuthProviderApi : ExternalAuthProviderApiBase
	{
		// Token: 0x06000039 RID: 57 RVA: 0x000024D0 File Offset: 0x000006D0
		public override async Task<ExternalAuthUserInfo> GetUserInfo(string accessCode)
		{
			ExternalAuthUserInfo result;
			using (HttpClient client = new HttpClient())
			{
				client.DefaultRequestHeaders.UserAgent.ParseAdd("Microsoft ASP.NET Core OAuth middleware");
				client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
				client.Timeout = TimeSpan.FromSeconds(30.0);
				client.MaxResponseContentBufferSize = 10485760L;
				HttpResponseMessage httpResponseMessage = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, MicrosoftAccountDefaults.UserInformationEndpoint)
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
					Name = MicrosoftAccountHelper.GetDisplayName(user),
					EmailAddress = MicrosoftAccountHelper.GetEmail(user),
					Surname = MicrosoftAccountHelper.GetSurname(user),
					Provider = "Microsoft",
					ProviderKey = MicrosoftAccountHelper.GetId(user)
				};
			}
			return result;
		}

		// Token: 0x04000011 RID: 17
		public const string Name = "Microsoft";
	}
}
