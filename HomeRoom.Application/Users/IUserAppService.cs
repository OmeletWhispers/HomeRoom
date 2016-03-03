using System.Threading.Tasks;
using Abp.Application.Services;
using HomeRoom.Users.Dto;

namespace HomeRoom.Users
{
    public interface IUserAppService : IApplicationService
    {
        Task ProhibitPermission(ProhibitPermissionInput input);

        Task RemoveFromRole(long userId, string roleName);
    }
}