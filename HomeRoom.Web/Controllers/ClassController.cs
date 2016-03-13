using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using HomeRoom.ClassEnrollment;
using HomeRoom.Datatables;
using HomeRoom.DataTableDto;
using HomeRoom.Web.Models.ClassEnrollment;
using Web.Extensions;

namespace HomeRoom.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ClassController : HomeRoomControllerBase
    {
        #region Private Fields

        private readonly IClassService _classService;

        #endregion

        #region Constructors
        public ClassController(IClassService classService)
        {
            _classService = classService;
        }
        #endregion

        #region Public Methods

        public ActionResult GetDataTable([ModelBinder(typeof (ModelBinderDataTableExtension))] IDataTableRequest request)
        {
            request.Length = request.Length < HomeRoomConsts.MinLength ? HomeRoomConsts.MinLength : request.Length;

            var sortedColumns = request.Columns.Where(x => x.IsOrdered).OrderBy(x => x.OrderNumber);

            var dataTableRequest = new DataTableRequestDto(request.Draw, request.Start, request.Length, sortedColumns.FirstOrDefault(), request.Search);

            var users =  _classService.GetAllTeacherClasses(dataTableRequest);

            return Json(users.ToDataTableResponse(), JsonRequestBehavior.AllowGet);
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
            var course = new Class {Id = model.Id, Name = model.Name, Subject = model.Subject};

            _classService.SaveClass(course);

            return Json(new {msg = "Save Successful"}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteClass(int classId)
        {
            _classService.DeleteClass(classId);

            return Json(new {msg = "Class has been deleted"}, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}