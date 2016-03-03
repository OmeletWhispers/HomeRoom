using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HomeRoom.MultiTenancy.Dto;

namespace HomeRoom.MultiTenancy
{
    public interface ITenantAppService : IApplicationService
    {
        ListResultOutput<TenantListDto> GetTenants();

        Task CreateTenant(CreateTenantInput input);
    }
}
