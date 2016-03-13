using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.Threading;
using HomeRoom.Datatables;
using HomeRoom.DataTableDto;
using HomeRoom.Users;

namespace HomeRoom.ClassEnrollment
{
    public class ClassService : HomeRoomAppServiceBase, IClassService
    {
        private readonly IRepository<Class> _classRepository;
        private readonly UserManager _userManager;

        public ClassService(IRepository<Class> classRepository, UserManager userManager)
        {
            _classRepository = classRepository;
            _userManager = userManager;
        }


        public DataTableResponseDto GetAllTeacherClasses(DataTableRequestDto dataTableRequest)
        {
            var userId = AbpSession.UserId;
            var search = dataTableRequest.Search;
            var sortedColumns = dataTableRequest.SortedColumns;
            var classes = userId.HasValue ? _classRepository.GetAll().Where(x => x.TeacherId == userId.Value) : _classRepository.GetAll();

            // searching
            if (search != null && !string.IsNullOrWhiteSpace(search.Value))
            {
                var searchTerm = search.Value.ToLower();

                classes = classes.Where(x => x.Name.ToLower().Contains(searchTerm) || x.Subject.ToLower().Contains(searchTerm));
            }

            // column sorting
            // default sorting
            if (sortedColumns == null)
            {
                classes = classes.OrderBy(x => x.Name);
            }
            else
            {
                switch (sortedColumns.Data)
                {
                    case "className":
                    {
                        classes = sortedColumns.SortDirection == ColumnViewModel.OrderDirection.Ascendant 
                                ? classes.OrderBy(x => x.Name) 
                                : classes.OrderByDescending(x => x.Name);
                    }
                        break;

                    case "subject":
                    {
                        classes = sortedColumns.SortDirection == ColumnViewModel.OrderDirection.Ascendant
                            ? classes.OrderBy(x => x.Subject)
                            : classes.OrderByDescending(x => x.Subject);
                    }
                        break;

                    case "students":
                    {
                        classes = sortedColumns.SortDirection == ColumnViewModel.OrderDirection.Ascendant
                            ? classes.OrderBy(x => x.Enrollments.Count)
                            : classes.OrderByDescending(x => x.Enrollments.Count);
                    }
                        break;
                }
            }

            var tableData = classes.Select(x => new
            {
                Id = x.Id,
                ClassName = x.Name,
                Subject = x.Subject,
                Students = x.Enrollments.Count
            }).ToList();

            var response = new DataTableResponseDto(dataTableRequest.Draw, tableData.Count, tableData.Count, tableData);

            return response;
        }

        public Class GetClassById(int classId)
        {
            var course = _classRepository.Get(classId);

            return course;
        }

        public void SaveClass(Class theClass)
        {
            // automatically set the teacher for the course to be the current logged in user
            // current logged in user will always be a teacher
            theClass.TeacherId = AbpSession.GetUserId();

            if (theClass.Id == 0)
            {
                _classRepository.Insert(theClass);
            }
            else
            {
                _classRepository.Update(theClass);
            }
        }

        public void DeleteClass(int classId)
        {
            var course = GetClassById(classId);

            _classRepository.Delete(course);
        }
    }
}
