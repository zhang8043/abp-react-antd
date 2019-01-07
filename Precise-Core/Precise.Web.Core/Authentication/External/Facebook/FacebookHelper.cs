using System;
using Newtonsoft.Json.Linq;

namespace Precise.Web.Authentication.External.Facebook
{
	// Token: 0x02000014 RID: 20
	public static class FacebookHelper
	{
		// Token: 0x06000048 RID: 72 RVA: 0x000027A0 File Offset: 0x000009A0
		public static string GetId(JObject user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return user.Value<string>("id");
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000027BB File Offset: 0x000009BB
		public static string GetAgeRangeMin(JObject user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return FacebookHelper.TryGetValue(user, "age_range", "min");
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000027DB File Offset: 0x000009DB
		public static string GetAgeRangeMax(JObject user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return FacebookHelper.TryGetValue(user, "age_range", "max");
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000027FB File Offset: 0x000009FB
		public static string GetBirthday(JObject user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return user.Value<string>("birthday");
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002816 File Offset: 0x00000A16
		public static string GetEmail(JObject user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return user.Value<string>("email");
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002831 File Offset: 0x00000A31
		public static string GetFirstName(JObject user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return user.Value<string>("first_name");
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0000284C File Offset: 0x00000A4C
		public static string GetGender(JObject user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return user.Value<string>("gender");
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002867 File Offset: 0x00000A67
		public static string GetLastName(JObject user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return user.Value<string>("last_name");
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002882 File Offset: 0x00000A82
		public static string GetLink(JObject user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return user.Value<string>("link");
		}

		// Token: 0x06000051 RID: 81 RVA: 0x0000289D File Offset: 0x00000A9D
		public static string GetLocation(JObject user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return FacebookHelper.TryGetValue(user, "location", "name");
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000028BD File Offset: 0x00000ABD
		public static string GetLocale(JObject user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return user.Value<string>("locale");
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000028D8 File Offset: 0x00000AD8
		public static string GetMiddleName(JObject user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return user.Value<string>("middle_name");
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000028F3 File Offset: 0x00000AF3
		public static string GetName(JObject user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return user.Value<string>("name");
		}

		// Token: 0x06000055 RID: 85 RVA: 0x0000290E File Offset: 0x00000B0E
		public static string GetTimeZone(JObject user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return user.Value<string>("timezone");
		}

		// Token: 0x06000056 RID: 86 RVA: 0x0000292C File Offset: 0x00000B2C
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
	}
}
