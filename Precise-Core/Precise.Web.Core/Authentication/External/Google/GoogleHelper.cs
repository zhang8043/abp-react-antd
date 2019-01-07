using System;
using Newtonsoft.Json.Linq;

namespace Precise.Web.Authentication.External.Google
{
	// Token: 0x02000012 RID: 18
	public static class GoogleHelper
	{
		// Token: 0x0600003D RID: 61 RVA: 0x0000256D File Offset: 0x0000076D
		public static string GetId(JObject user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return user.Value<string>("id");
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002588 File Offset: 0x00000788
		public static string GetName(JObject user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return user.Value<string>("displayName");
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000025A3 File Offset: 0x000007A3
		public static string GetGivenName(JObject user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return GoogleHelper.TryGetValue(user, "name", "givenName");
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000025C3 File Offset: 0x000007C3
		public static string GetFamilyName(JObject user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return GoogleHelper.TryGetValue(user, "name", "familyName");
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000025E3 File Offset: 0x000007E3
		public static string GetProfile(JObject user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return user.Value<string>("url");
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000025FE File Offset: 0x000007FE
		public static string GetEmail(JObject user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return GoogleHelper.TryGetFirstValue(user, "emails", "value");
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002620 File Offset: 0x00000820
		private static string TryGetValue(JObject user, string propertyName, string subProperty)
		{
			JToken jtoken;
			if (user.TryGetValue(propertyName, out jtoken))
			{
				JObject jobject = JObject.Parse(jtoken.ToString());
				if (jobject != null && jobject.TryGetValue(subProperty, out jtoken))
				{
					return jtoken.ToString();
				}
			}
			return null;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x0000265C File Offset: 0x0000085C
		private static string TryGetFirstValue(JObject user, string propertyName, string subProperty)
		{
			JToken jtoken;
			if (user.TryGetValue(propertyName, out jtoken))
			{
				JArray jarray = JArray.Parse(jtoken.ToString());
				if (jarray != null && jarray.Count > 0)
				{
					JObject jobject = JObject.Parse(jarray.First.ToString());
					if (jobject != null && jobject.TryGetValue(subProperty, out jtoken))
					{
						return jtoken.ToString();
					}
				}
			}
			return null;
		}
	}
}
