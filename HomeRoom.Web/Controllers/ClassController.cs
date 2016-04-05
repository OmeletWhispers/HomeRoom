using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.Web.Mvc.Authorization;
using HomeRoom.ClassEnrollment;
using HomeRoom.ClassEnrollment.Dtos;
using HomeRoom.Datatables;
using HomeRoom.DataTableDto;
using HomeRoom.Enumerations;
using HomeRoom.Users;
using HomeRoom.Users.Dto;
using HomeRoom.Web.Models.ClassEnrollment;
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

        #endregion

        #region Constructors
        public ClassController(IClassService classService, UserManager userManager, IUserAppService userAppService, IUnitOfWorkManager unitOfWorkManager)
        {
            _classService = classService;
            _userManager = userManager;
            _userAppService = userAppService;
            _unitOfWorkManager = unitOfWorkManager;
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

        [HttpGet]
        public PartialViewResult EnrollStudent(int classId, int? studentId)
        {
            if (studentId.HasValue)
            {
                var student = _userAppService.GetStudentById(studentId.Value);
                var enrollModel = new EnrollStudentViewModel(student, classId);

                return PartialView("Forms/_EnrollStudentForm", enrollModel);
            }

            var model = new EnrollStudentViewModel { ClassId = classId };

            return PartialView("Forms/_EnrollStudentForm", model);
        }

        [HttpPost]
        [UnitOfWork]
        public virtual JsonResult EnrollStudent(EnrollStudentViewModel model)
        {
            CheckModelState();

            var email = model.StudentEmail;
            var studentModel = new EnrollStudentDto(model.ClassId, new UserDto(model.StudentFirstName, model.StudentLastName, email));

            var isEnrolled = _classService.IsStudentEnrolled(studentModel);
            if (isEnrolled)
            {
                return Json(new {error = true, msg = "This student is already enrolled in this class."});
            }

            _classService.EnrollStudent(studentModel);
            

            return Json(new { error = false, msg = "Student has been enrolled" });
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
        #endregion
    }
}