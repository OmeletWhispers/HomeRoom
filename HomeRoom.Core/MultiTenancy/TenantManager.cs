using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using HomeRoom.Authorization.Roles;
using HomeRoom.Editions;
using HomeRoom.Users;

namespace HomeRoom.MultiTenancy
{
    public class TenantManager : AbpTenantManager<Tenant, Role, User>
    {
        public TenantManager(
            IRepository<Tenant> tenantRepository, 
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository, 
            EditionManager editionManager) 
            : base(
                tenantRepository, 
                tenantFeatureRepository, 
                editionManager
            )
        {
        }
    }
}