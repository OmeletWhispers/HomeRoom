using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using HomeRoom.ClassEnrollment;
using HomeRoom.Datatables;
using HomeRoom.DataTableDto;
using HomeRoom.Users;
using Web.Extensions;
using HomeRoom.Web.Models.Student;
using HomeRoom.Gradebook;
using HomeRoom.Web.Models.Gradebook;

namespace HomeRoom.Web.Controllers
{
    [AbpMvcAuthorize]
    public class StudentController : HomeRoomControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly IClassService _classService;
        private readonly IAssignmentService _assignmentService;
        private readonly IAssignmentTypeService _assignmentTypeService;

        public StudentController(IUserAppService userAppService, IClassService classService, IAssignmentService assignmentService, IAssignmentTypeService assignmentTypeService)
        {
            _userAppService = userAppService;
            _classService = classService;
            _assignmentService = assignmentService;
            _assignmentTypeService = assignmentTypeService;
        }

        public ActionResult GetDataTable([ModelBinder(typeof(ModelBinderDataTableExtension))] IDataTableRequest request)
        {
            request.Length = request.Length < HomeRoomConsts.MinLength ? HomeRoomConsts.MinLength : request.Length;

            var sortedColumns = request.Columns.Where(x => x.IsOrdered).OrderBy(x => x.OrderNumber);

            var dataTableRequest = new DataTableRequestDto(request.Draw, request.Start, request.Length, sortedColumns.FirstOrDefault(), request.Search);

            var users = _classService.GetAllStudentClasses(dataTableRequest);

            return Json(users.ToDataTableResponse(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewClass(int classId)
        {
            var course = _classService.GetClassById(classId);
            var courseModel = new ViewClassViewModel
            {
                ClassId = classId,
                ClassName = course.Name
            };

            return View("ViewClass", courseModel);
        }

        [ChildActionOnly]
        public PartialViewResult StudentClassDashboard(int classId)
        {
            var upcomingAssignments = _assignmentService.GetUpcomingAssignments(classId);

            return PartialView("_StudentClassDashboard", upcomingAssignments);
        }

        [ChildActionOnly]
        public PartialViewResult StudentClassAssignments()
        {
            return PartialView("_StudentClassAssignments");
        }

        [ChildActionOnly]
        public PartialViewResult StudentClassGradeBook(int classId)
        {
            var assignmentsTypes = _assignmentTypeService.GetAllAssignmentTypes(classId).OrderBy(x => x.Name).Select(x => x.Name);
            var model = new GradebookViewModel(assignmentsTypes);

            return PartialView("_StudentClassGradeBook", model);
        }

    }
}