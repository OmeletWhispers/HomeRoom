using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using HomeRoom.Datatables;
using HomeRoom.Users.Dto;

namespace HomeRoom.Users
{
    /* THIS IS JUST A SAMPLE. */
    public class UserAppService : HomeRoomAppServiceBase, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly IPermissionManager _permissionManager;

        public UserAppService(UserManager userManager, IPermissionManager permissionManager)
        {
            _userManager = userManager;
            _permissionManager = permissionManager;
        }

        public async Task ProhibitPermission(ProhibitPermissionInput input)
        {
            var user = await _userManager.GetUserByIdAsync(input.UserId);
            var permission = _permissionManager.GetPermission(input.PermissionName);

            await _userManager.ProhibitPermissionAsync(user, permission);
        }

        //Example for primitive method parameters.
        public async Task RemoveFromRole(long userId, string roleName)
        {
            CheckErrors(await _userManager.RemoveFromRoleAsync(userId, roleName));
        }

        public List<User> GetAllUsers(int pageIndex, int pageSize, ColumnViewModel sortedColumns = null, SearchViewModel search = null)
        {
            var users = _userManager.Users;

            // searching
            if (search != null && !string.IsNullOrWhiteSpace(search.Value))
            {
                var searchTerm = search.Value.ToLower();

                users =
                    users.Where(
                        x => x.Name.ToLower().Contains(searchTerm) || x.Surname.ToLower().Contains(searchTerm) || x.EmailAddress.ToLower().Contains(searchTerm));
            }

            return users.ToList();

        }
    }
}