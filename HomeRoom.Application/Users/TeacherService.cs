using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HomeRoom.Membership;

namespace HomeRoom.Users
{
    public class TeacherService : HomeRoomAppServiceBase, ITeacherService
    {
        #region Private Fields

        private readonly IRepository<Teacher, long> _teacherRepo;

        #endregion

        #region Constructors
        public TeacherService(IRepository<Teacher, long> teacherRepo)
        {
            _teacherRepo = teacherRepo;
        }
        #endregion

        #region Public Methods

        public async Task InsertTeacher(long userId)
        {
            var teacher = new Teacher
            {
                Id = userId
            };

            await _teacherRepo.InsertAsync(teacher);
        }

        public async Task UpdateTeacher(long userId)
        {
            var teacher = _teacherRepo.GetAsync(userId);

            await _teacherRepo.UpdateAsync(await teacher);
        }

        public bool IsUserTeacher(long userId)
        {
            return _teacherRepo.GetAll().Any(x => x.Id == userId);
        }

        #endregion
    }
}
