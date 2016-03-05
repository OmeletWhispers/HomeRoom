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
    }
}
