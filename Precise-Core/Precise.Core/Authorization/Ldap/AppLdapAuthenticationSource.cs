using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using Precise.Authorization.Users;
using Precise.MultiTenancy;

namespace Precise.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}