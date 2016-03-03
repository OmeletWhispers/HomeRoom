using Abp.Authorization.Roles;
using HomeRoom.MultiTenancy;
using HomeRoom.Users;

namespace HomeRoom.Authorization.Roles
{
    public class Role : AbpRole<Tenant, User>
    {

    }
}