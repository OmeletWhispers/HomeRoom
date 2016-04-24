﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.Web.Mvc.Authorization;
using Castle.Core.Internal;
using HomeRoom.ClassEnrollment;
using HomeRoom.ClassEnrollment.Dtos;
using HomeRoom.Datatables;
using HomeRoom.DataTableDto;
using HomeRoom.Enumerations;
using HomeRoom.Gradebook;
using HomeRoom.TestGenerator;
using HomeRoom.Users;
using HomeRoom.Users.Dto;
using HomeRoom.Web.Models;
using HomeRoom.Web.Models.ClassEnrollment;
using HomeRoom.Web.Models.Gradebook;
using HomeRoom.Web.Models.TestGenerator;
using Microsoft.AspNet.Identity;
using Web.Extensions;

namespace HomeRoom.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ClassController : HomeRoomControllerBase
    {
        #region Private Fields

        private readonly IClassService _classService;
        private readonly UserManager _userManager;
        private readonly IUserAppService _userAppService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IAssignmentTypeService _assignmentTypeService;
        private readonly IAssignmentService _assignmentService;
        private readonly IGradeBookService _gradeBookService;
        private readonly IQuestionService _questionService;
        private readonly ISubjectService _subjectService;

        #endregion

        #region Constructors
        public ClassController(IClassService classService, UserManager userManager, IUserAppService userAppService, IUnitOfWorkManager unitOfWorkManager, IAssignmentTypeService assignmentTypeService, IGradeBookService gradeBookService, IQuestionService questionService, IAssignmentService assignmentService, ISubjectService subjectService)
        {
            _classService = classService;
            _userManager = userManager;
            _userAppService = userAppService;
            _unitOfWorkManager = unitOfWorkManager;
            _assignmentTypeService = assignmentTypeService;
            _gradeBookService = gradeBookService;
            _questionService = questionService;
            _assignmentService = assignmentService;
            _subjectService = subjectService;
        }
        #endregion

        #region Public Methods

        public ActionResult GetDataTable([ModelBinder(typeof(ModelBinderDataTableExtension))] IDataTableRequest request)
        {
            request.Length = request.Length < HomeRoomConsts.MinLength ? HomeRoomConsts.MinLength : request.Length;

            var sortedColumns = request.Columns.Where(x => x.IsOrdered).OrderBy(x => x.OrderNumber);

            var dataTableRequest = new DataTableRequestDto(request.Draw, request.Start, request.Length, sortedColumns.FirstOrDefault(), request.Search);

            var users = _classService.GetAllTeacherClasses(dataTableRequest);

            return Json(users.ToDataTableResponse(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEnrollmentDataTable([ModelBinder(typeof(ModelBinderDataTableExtension))] IDataTableRequest request, int classId)
        {
            request.Length = request.Length < HomeRoomConsts.MinLength ? HomeRoomConsts.MinLength : request.Length;
            var sortedColumns = request.Columns.Where(x => x.IsOrdered).OrderBy(x => x.OrderNumber);
            var dataTableRequest = new DataTableRequestDto(request.Draw, request.Start, request.Length, sortedColumns.FirstOrDefault(), request.Search);

            var enrollments = _classService.GetAllEnrollments(classId, dataTableRequest);

            return Json(enrollments.ToDataTableResponse(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetGradeBookDataTable([ModelBinder(typeof (ModelBinderDataTableExtension))] IDataTableRequest request, int classId)
        {
            request.Length = request.Length < HomeRoomConsts.MinLength ? HomeRoomConsts.MinLength : request.Length;
            var sortedColumns = request.Columns.Where(x => x.IsOrdered).OrderBy(x => x.OrderNumber);
            var dataTableRequest = new DataTableRequestDto(request.Draw, request.Start, request.Length, sortedColumns.FirstOrDefault(), request.Search);

            var gradeBook = _gradeBookService.GetAllClassGrades(classId, dataTableRequest);

            return Json(gradeBook.ToDataTableResponse(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Class(int? classId)
        {
            var model = new ClassFormViewModel();

            // creating a new class
            if (!classId.HasValue) return PartialView("Forms/_ClassForm", model);

            // gave a class id, so editing a class
            // get this class by its id and set view model to its data
            var course = _classService.GetClassById(classId.Value);
            model.Name = course.Name;
            model.Id = course.Id;
            model.Subject = course.Subject;

            return PartialView("Forms/_ClassForm", model);
        }

        [HttpPost]
        public JsonResult Class(ClassFormViewModel model)
        {
            var course = new Class { Id = model.Id, Name = model.Name, Subject = model.Subject };

            _classService.SaveClass(course);

            return Json(new { msg = "Save Successful" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteClass(int classId)
        {
            _classService.DeleteClass(classId);

            return Json(new { msg = "Class has been deleted" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemoveStudent(int classId, long studentId)
        {
            _classService.UnenrollStudent(classId, studentId);

            return Json(new {msg = "Student has been removed from class"}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult EnrollStudent(int classId, long? studentId)
        {
            if (studentId.HasValue)
            {
                var student = _userAppService.GetStudentById(studentId.Value);
                var enrollModel = new EnrollStudentViewModel(student, classId);

                return PartialView("Forms/_EnrollStudentForm", enrollModel);
            }

            var model = new EnrollStudentViewModel { ClassId = classId, StudentId = 0};

            return PartialView("Forms/_EnrollStudentForm", model);
        }

        [HttpPost]
        [UnitOfWork]
        public virtual JsonResult EnrollStudent(EnrollStudentViewModel model)
        {
            CheckModelState();

            var email = model.StudentEmail;
            var studentModel = new EnrollStudentDto(model.ClassId, new UserDto(model.StudentId, model.StudentFirstName, model.StudentLastName, email));

            var isEnrolled = _classService.IsStudentEnrolled(studentModel);

            // we already have a student enrolled in the class with this email and we are trying to add them
            if (isEnrolled && model.StudentId == 0)
            {
                return Json(new {error = true, msg = "This student is already enrolled in this class."});
            }

            // are we adding/editing the parent attached to this student?
            if (!model.ParentFirstName.IsNullOrEmpty())
            {
                if (!model.ParentId.HasValue) model.ParentId = 0;
                var userModel = new UserDto(model.ParentId.Value, model.ParentFirstName, model.ParentLastName, model.ParentEmailAddress);
                _userAppService.SaveParent(model.StudentId, userModel);
            }

            // student was not enrolled, enroll them
            if (!isEnrolled)
            {
                _classService.EnrollStudent(studentModel);
            }
            // editing a student
            else if (model.StudentId != 0)
            {
                var userModel = new UserDto(model.StudentId, model.StudentFirstName, model.StudentLastName, model.StudentEmail);
                _userAppService.UpdateUser(userModel);

                return Json(new {error = false, msg = "Student has been updated"});

            }
            

            return Json(new { error = false, msg = "Student has been enrolled" });
        }

        [HttpGet]
        public JsonResult GetClasGradebookColumns(int classId)
        {
            // get the columsn that are needed
            var studentColumn = new DataTableColumnModel("StudentName");
            var gradeColumn = new DataTableColumnModel("CurrentGrade");
            var assignmentTypes = _assignmentTypeService.GetAllAssignmentTypes(classId).OrderBy(x => x.Name).Select(x => new DataTableColumnModel(x.Name));

            // put all these columns in the order needed
            var gradeBookColumns = new List<DataTableColumnModel> {studentColumn};
            gradeBookColumns.AddRange(assignmentTypes);
            gradeBookColumns.Add(gradeColumn);

            return Json(gradeBookColumns, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public PartialViewResult ManageClassDashboard(int classId)
        {
            return PartialView("_ManageClassDashboard");
        }

        [ChildActionOnly]
        public PartialViewResult ManageClassEnrollments(int classId)
        {
            return PartialView("_ManageClassEnrollments");
        }

        [ChildActionOnly]
        public PartialViewResult ManageClassAssignments()
        {
            return PartialView("_ManageClassAssignments");
        }

        [ChildActionOnly]
        public PartialViewResult ManageClassAssignmentTypes()
        {
            return PartialView("_ManageClassAssignmentTypes");
        }

        [ChildActionOnly]
        public PartialViewResult ManageClassGradeBook(int classId)
        {
            var assignmentsTypes = _assignmentTypeService.GetAllAssignmentTypes(classId).OrderBy(x => x.Name).Select(x => x.Name);
            var model = new GradebookViewModel(assignmentsTypes);

            return PartialView("_ManageClassGradeBook", model);
        }

        [ChildActionOnly]
        public PartialViewResult ManageTestGenerator(int classId)
        {
            var assignments = _assignmentService.GetCreatedAssignments(classId);
            var subjects = _subjectService.GetAllSubjects();

            var model = new TestGeneratorViewModel(assignments, subjects);

            return PartialView("_TestGenerator", model);
        }
        #endregion


    }
}