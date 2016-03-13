using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;

namespace HomeRoom.Users
{
    public interface ITeacherService : IApplicationService
    {
        /// <summary>
        /// Inserts the teacher.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task InsertTeacher(long userId);

        /// <summary>
        /// Updates the teacher.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task UpdateTeacher(long userId);

        /// <summary>
        /// Determines whether [is user teacher] [the specified user identifier].
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        bool IsUserTeacher(long userId);
    }
}
