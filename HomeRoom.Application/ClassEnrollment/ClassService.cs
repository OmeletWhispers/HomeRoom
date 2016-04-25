﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.Threading;
using HomeRoom.ClassEnrollment.Dtos;
using HomeRoom.Datatables;
using HomeRoom.DataTableDto;
using HomeRoom.Enumerations;
using HomeRoom.Users;
using Microsoft.AspNet.Identity;

namespace HomeRoom.ClassEnrollment
{
    public class ClassService : HomeRoomAppServiceBase, IClassService
    {
        #region Private Fields

        private readonly IRepository<Class> _classRepository;
        private readonly IRepository<Enrollment> _enrollmentRepository;
        private readonly UserManager _userManager;
        private readonly IUserAppService _userAppService;

        #endregion


        #region Constructors

        public ClassService(IRepository<Class> classRepository, UserManager userManager, IRepository<Enrollment> enrollmentRepository, IUserAppService userAppService)
        {
            _classRepository = classRepository;
            _userManager = userManager;
            _enrollmentRepository = enrollmentRepository;
            _userAppService = userAppService;
        }

        #endregion



        #region Pubic Methods

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
                Students = x.Enrollments.Count,
                CourseUrl = "Teacher/ManageClass?classId=" + x.Id
            }).ToList();

            var response = new DataTableResponseDto(dataTableRequest.Draw, tableData.Count, tableData.Count, tableData);

            return response;
        }

        public DataTableResponseDto GetAllEnrollments(int classId, DataTableRequestDto dataTableRequest)
        {
            var search = dataTableRequest.Search;
            var sortedColumns = dataTableRequest.SortedColumns;

            var enrollments = _enrollmentRepository.GetAll().Where(x => x.ClassId == classId);

            // searching
            if (search != null && !string.IsNullOrWhiteSpace(search.Value))
            {
                var searchTerm = search.Value.ToLower();

                enrollments = enrollments.Where(x => x.Student.Account.Name.ToLower().Contains(searchTerm) || x.Student.Account.Surname.ToLower().Contains(searchTerm));
            }

            if (sortedColumns == null)
            {
                enrollments = enrollments.OrderBy(x => x.Student.Account.Name);
            }
            else
            {
                switch (sortedColumns.Data)
                {
                    case "studentName":
                        enrollments = sortedColumns.SortDirection == ColumnViewModel.OrderDirection.Ascendant
                            ? enrollments.OrderBy(x => x.Student.Account.Name)
                            : enrollments.OrderByDescending(x => x.Student.Account.Name);
                        break;
                }

            }

            var tableData = enrollments.Select(x => new
            {
                Id = x.StudentId,
                StudentName = x.Student.Account.Name + " " + x.Student.Account.Surname
            }).ToList();

            var response = new DataTableResponseDto(dataTableRequest.Draw, tableData.Count, tableData.Count, tableData);

            return response;
        }

        public List<ParentStudentClassesDto> GetStudentClasses(long studentId)
        {
            var classes = _userManager.FindById(studentId).Student.Enrollments.Select(x => x.Class);

            var studentClasses = classes.Select(x => new ParentStudentClassesDto
            {
                Id = x.Id,
                ClassName = x.Name,
                Teacher = x.Teacher.Account.Name + " " + x.Teacher.Account.Surname
            }).ToList();

            return studentClasses;
        }

        public IEnumerable<User> GetAllEnrollments(int classId)
        {
            var enrollments = _enrollmentRepository.GetAll().Where(x => x.ClassId == classId).Select(x => x.Student.Account);

            return enrollments.ToList();
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

        public void UnenrollStudent(int classId, long studentId)
        {
            var enrollment = _enrollmentRepository.Single(x => x.StudentId == studentId && x.ClassId == classId);

            _enrollmentRepository.Delete(enrollment);
        }

        public bool IsStudentEnrolled(EnrollStudentDto enrolledStudent)
        {
            var student = _userManager.FindByEmail(enrolledStudent.User.Email);

            // if the student is not null check to see if they are enrolled and return that value
            return student != null && _enrollmentRepository.GetAll().Any(x => x.StudentId == student.Id && x.ClassId == enrolledStudent.ClassId);
        }

        public void EnrollStudent(EnrollStudentDto enrollStudent)
        {
            // check to see if their is an account for this user already
            // if so just make a new enrollment for this user
            if (_userAppService.HasStudentAccount(enrollStudent.User.Email))
            {
                var user = _userManager.FindByEmail(enrollStudent.User.Email);

                var enrollment = new Enrollment
                {
                    ClassId = enrollStudent.ClassId,
                    StudentId = user.Id
                };
                _enrollmentRepository.Insert(enrollment);
            }
            // no account found, create one then enroll the student
            else
            {
                var user = new User
                {
                    EmailAddress = enrollStudent.User.Email.ToLower(),
                    UserName = enrollStudent.User.Email.ToLower(),
                    TenantId = AbpSession.GetTenantId(),
                    Name = enrollStudent.User.FirstName,
                    Surname = enrollStudent.User.LastName,
                    AccountType = AccountType.Student,
                    Gender = Gender.Male,
                    IsActive = true,
                    Password = new PasswordHasher().HashPassword(Helpers.Helpers.GenerateRandomPassword(8))
                };

                CheckErrors(_userManager.Create(user));
                _userAppService.InsertStudent(user.Id);

                var enrollment = new Enrollment
                {
                    ClassId = enrollStudent.ClassId,
                    StudentId = user.Id
                };
                _enrollmentRepository.Insert(enrollment);
            }
        }

        #endregion

    }
}
