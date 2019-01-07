using Abp.Authorization;
using Precise.Authorization.Roles;
using Precise.Authorization.Users;

namespace Precise.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
