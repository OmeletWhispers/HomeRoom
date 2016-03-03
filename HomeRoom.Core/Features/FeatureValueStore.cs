using Abp.Application.Features;
using HomeRoom.Authorization.Roles;
using HomeRoom.MultiTenancy;
using HomeRoom.Users;

namespace HomeRoom.Features
{
    public class FeatureValueStore : AbpFeatureValueStore<Tenant, Role, User>
    {
        public FeatureValueStore(TenantManager tenantManager)
            : base(tenantManager)
        {
        }
    }
}