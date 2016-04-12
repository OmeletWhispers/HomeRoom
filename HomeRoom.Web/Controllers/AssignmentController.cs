using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HomeRoom.Datatables;
using HomeRoom.DataTableDto;
using HomeRoom.Gradebook;
using HomeRoom.GradeBook;
using HomeRoom.Web.Models.Gradebook;
using Web.Extensions;

namespace HomeRoom.Web.Controllers
{
    public class AssignmentController : HomeRoomControllerBase
    {
        #region Private Fields

        private readonly IAssignmentService _assignmentService;
        private readonly IAssignmentTypeService _assignmentTypeService;
        #endregion

        #region Constructors
        public AssignmentController(IAssignmentService assignmentService, IAssignmentTypeService assignmentTypeService)
        {
            _assignmentService = assignmentService;
            _assignmentTypeService = assignmentTypeService;
        }

        #endregion

        #region Public Methods


        public ActionResult GetClassAssignmentsDataTable([ModelBinder(typeof(ModelBinderDataTableExtension))] IDataTableRequest request, int classId)
        {
            request.Length = request.Length < HomeRoomConsts.MinLength ? HomeRoomConsts.MinLength : request.Length;
            var sortedColumns = request.Columns.Where(x => x.IsOrdered).OrderBy(x => x.OrderNumber);
            var dataTableRequest = new DataTableRequestDto(request.Draw, request.Start, request.Length, sortedColumns.FirstOrDefault(), request.Search);

            var enrollments = _assignmentService.GetAllClassAssignments(classId, dataTableRequest);

            return Json(enrollments.ToDataTableResponse(), JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public PartialViewResult Assignment(int classId, int? assignmentId)
        {
            var assignmentTypes = _assignmentTypeService.GetAllAssignmentTypes(classId);

            if (assignmentId.HasValue)
            {
                var assignment = _assignmentService.GetById(assignmentId.Value);
                var model = new ClassAssignmentViewModel(assignment, assignmentTypes);

                return PartialView("Forms/_AssignmentForm", model);
            }
            else
            {
                var model = new ClassAssignmentViewModel(classId, assignmentTypes);
                return PartialView("Forms/_AssignmentForm", model);
            }


        }

        [HttpPost]
        public JsonResult Assignment(ClassAssignmentViewModel model)
        {
            var assignment = new Assignment
            {
                Id = model.AssignmentId,
                AssignmentTypeId = model.AssignmentTypeId,
                ClassId = model.ClassId,
                Name = model.Name,
                Description = model.Description,
                Status = model.Status,
                StartDate = model.StartDate,
                DueDate = model.DueDate
            };

            _assignmentService.SaveAssignment(assignment);

            return Json(new {msg = "Assignment has been successfuly saved!", error = false});
        }

        #endregion
    }
}