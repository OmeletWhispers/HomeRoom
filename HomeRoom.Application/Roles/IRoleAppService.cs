using System.Threading.Tasks;
using Abp.Application.Services;
using HomeRoom.Roles.Dto;

namespace HomeRoom.Roles
{
    public interface IRoleAppService : IApplicationService
    {
        Task UpdateRolePermissions(UpdateRolePermissionsInput input);
    }
}
