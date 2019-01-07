using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Precise.Web.Authentication.External.OpenIdConnect
{
	// Token: 0x0200000E RID: 14
	public class OpenIdConnectAuthProviderApi : ExternalAuthProviderApiBase
	{
		// Token: 0x06000031 RID: 49 RVA: 0x0000237C File Offset: 0x0000057C
		public override async Task<ExternalAuthUserInfo> GetUserInfo(string token)
		{
			string text = base.ProviderInfo.AdditionalParams["Authority"];
			if (string.IsNullOrEmpty(text))
			{
				throw new ApplicationException("Authentication:OpenId:Issuer configuration is required.");
			}
			ConfigurationManager<OpenIdConnectConfiguration> configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(text + "/.well-known/openid-configuration", new OpenIdConnectConfigurationRetriever(), new HttpDocumentRetriever());
			JwtSecurityToken jwtSecurityToken = await this.ValidateToken(token, text, configurationManager, default(CancellationToken));
			string value = jwtSecurityToken.Claims.First((Claim c) => c.Type == "name").Value;
			string value2 = jwtSecurityToken.Claims.First((Claim c) => c.Type == "unique_name").Value;
			string[] array = value.Split(new char[]
			{
				' '
			});
			return new ExternalAuthUserInfo
			{
				Provider = "OpenIdConnect",
				ProviderKey = jwtSecurityToken.Subject,
				Name = array[0],
				Surname = array[1],
				EmailAddress = value2
			};
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000023CC File Offset: 0x000005CC
		private async Task<JwtSecurityToken> ValidateToken(string token, string issuer, IConfigurationManager<OpenIdConnectConfiguration> configurationManager, CancellationToken ct = default(CancellationToken))
		{
			if (string.IsNullOrEmpty(token))
			{
				throw new ArgumentNullException("token");
			}
			if (string.IsNullOrEmpty(issuer))
			{
				throw new ArgumentNullException("issuer");
			}
			ICollection<SecurityKey> signingKeys = (await configurationManager.GetConfigurationAsync(ct)).SigningKeys;
			TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidIssuer = issuer,
				ValidateIssuerSigningKey = true,
				IssuerSigningKeys = signingKeys,
				ValidateLifetime = true,
				ClockSkew = TimeSpan.FromMinutes(5.0),
				ValidateAudience = false
			};
			SecurityToken securityToken;
			ClaimsPrincipal claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out securityToken);
			if (base.ProviderInfo.ClientId != claimsPrincipal.Claims.First((Claim c) => c.Type == "aud").Value)
			{
				throw new ApplicationException("ClientId couldn't verified.");
			}
			return (JwtSecurityToken)securityToken;
		}

		// Token: 0x04000010 RID: 16
		public const string Name = "OpenIdConnect";
	}
}
