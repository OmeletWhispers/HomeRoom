using Abp.Authorization;
using HomeRoom.Authorization.Roles;
using HomeRoom.MultiTenancy;
using HomeRoom.Users;

namespace HomeRoom.Authorization
{
    public class PermissionChecker : PermissionChecker<Tenant, Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
