using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using HomeRoom.Datatables;
using HomeRoom.DataTableDto;
using HomeRoom.Membership;
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
        /// <returns></returns>
        DataTableResponseDto GetAllUsers(DataTableRequestDto dataTableRequest);

        /// <summary>
        /// Determines whether [is user registered] [the specified user name].
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        bool IsUserRegistered(string userName);

        /// <summary>
        /// Gets the student by identifier.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <returns></returns>
        UserDto GetStudentById(long studentId);

        /// <summary>
        /// Determines whether [has student account] [the specified email].
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        bool HasStudentAccount(string email);

        /// <summary>
        /// Inserts the student.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        void InsertStudent(long userId);
    }
}