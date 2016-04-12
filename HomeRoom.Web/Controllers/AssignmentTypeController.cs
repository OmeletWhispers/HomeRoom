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
    public class AssignmentTypeController : HomeRoomControllerBase
    {
        private readonly IAssignmentTypeService _assignmentTypeService;

        public AssignmentTypeController(IAssignmentTypeService assignmentTypeService)
        {
            _assignmentTypeService = assignmentTypeService;
        }


        public ActionResult GetClassAssignmentTypesDataTable([ModelBinder(typeof(ModelBinderDataTableExtension))] IDataTableRequest request, int classId)
        {
            request.Length = request.Length < HomeRoomConsts.MinLength ? HomeRoomConsts.MinLength : request.Length;
            var sortedColumns = request.Columns.Where(x => x.IsOrdered).OrderBy(x => x.OrderNumber);
            var dataTableRequest = new DataTableRequestDto(request.Draw, request.Start, request.Length, sortedColumns.FirstOrDefault(), request.Search);

            var enrollments = _assignmentTypeService.GetAllClassAssignmentTypes(classId, dataTableRequest);

            return Json(enrollments.ToDataTableResponse(), JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public PartialViewResult AssignmentType(int classId, int? id)
        {
            if (id.HasValue)
            {
                var assignmentType = _assignmentTypeService.GetAssignmentTypeById(id.Value);

                return PartialView("Forms/_AssignmentTypeForm", new AssignmentTypeViewModel(assignmentType));
            }

            var model = new AssignmentTypeViewModel(classId);
            return PartialView("Forms/_AssignmentTypeForm", model);
        }

        [HttpPost]
        public JsonResult AssignmentType(AssignmentTypeViewModel assignmentTypeViewModel)
        {
            var assignmentType = new AssignmentType
            {
                Id = assignmentTypeViewModel.Id,
                ClassId = assignmentTypeViewModel.ClassId,
                Name = assignmentTypeViewModel.Name,
                Percentage = assignmentTypeViewModel.PercentageValue / 100.0
            };

            _assignmentTypeService.SaveAssignmentType(assignmentType);

            return Json(new {msg = string.Format("{0} has been saved!", assignmentType.Name), error = false});
        }
    }
}