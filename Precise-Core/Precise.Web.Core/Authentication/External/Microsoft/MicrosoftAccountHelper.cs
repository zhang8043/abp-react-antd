using System;
using Newtonsoft.Json.Linq;

namespace Precise.Web.Authentication.External.Microsoft
{
	// Token: 0x0200000F RID: 15
	public static class MicrosoftAccountHelper
	{
		// Token: 0x06000034 RID: 52 RVA: 0x0000243A File Offset: 0x0000063A
		public static string GetId(JObject user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return user.Value<string>("id");
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002455 File Offset: 0x00000655
		public static string GetDisplayName(JObject user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return user.Value<string>("displayName");
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002470 File Offset: 0x00000670
		public static string GetGivenName(JObject user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return user.Value<string>("givenName");
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000248B File Offset: 0x0000068B
		public static string GetSurname(JObject user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return user.Value<string>("surname");
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000024A6 File Offset: 0x000006A6
		public static string GetEmail(JObject user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return user.Value<string>("mail") ?? user.Value<string>("userPrincipalName");
		}
	}
}
