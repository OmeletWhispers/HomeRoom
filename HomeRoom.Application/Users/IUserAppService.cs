using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using HomeRoom.Datatables;
using HomeRoom.Users.Dto;

namespace HomeRoom.Users
{
    public interface IUserAppService : IApplicationService
    {
        /// <summary>
        /// Prohibits the permission.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        Task ProhibitPermission(ProhibitPermissionInput input);

        /// <summary>
        /// Removes from role.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        Task RemoveFromRole(long userId, string roleName);

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="sortedColumns">The sorted columns.</param>
        /// <param name="search">The search.</param>
        /// <returns></returns>
        List<User> GetAllUsers(int pageIndex, int pageSize, ColumnViewModel sortedColumns = null, SearchViewModel search = null);
    }
}