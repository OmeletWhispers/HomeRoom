using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using HomeRoom.Datatables;
using HomeRoom.DataTableDto;
using HomeRoom.TestGenerator;
using HomeRoom.Web.Models.TestGenerator;
using Web.Extensions;

namespace HomeRoom.Web.Controllers
{
    [AbpMvcAuthorize]
    public class SubjectController : HomeRoomControllerBase
    {
        #region Private Fields

        private readonly ISubjectService _subjectService;

        #endregion

        #region Constructors

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        #endregion

        #region Public Methods
        public ActionResult GetDataTable([ModelBinder(typeof(ModelBinderDataTableExtension))] IDataTableRequest request)
        {
            request.Length = request.Length < HomeRoomConsts.MinLength ? HomeRoomConsts.MinLength : request.Length;

            var sortedColumns = request.Columns.Where(x => x.IsOrdered).OrderBy(x => x.OrderNumber);

            var dataTableRequest = new DataTableRequestDto(request.Draw, request.Start, request.Length, sortedColumns.FirstOrDefault(), request.Search);

            var users = _subjectService.GetAllSubjects(dataTableRequest);

            return Json(users.ToDataTableResponse(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult Subject(int? id)
        {
            if (id.HasValue)
            {
                var subject = _subjectService.GetSubjectById(id.Value);
                var model = new SubjectViewModel(subject);

                return PartialView("Forms/_SubjectForm", model);

            }
            else
            {
                var model = new SubjectViewModel();
                return PartialView("Forms/_SubjectForm", model);
            }
        }


        [HttpPost]
        public JsonResult Subject(SubjectViewModel model)
        {
            if (!AbpSession.UserId.HasValue)
                return Json(new {error = true, msg = "You must be logged in to save a subject"});

            var subject = new Subject
            {
                TeacherId = AbpSession.UserId.Value,
                Name = model.Name,
                Id = model.Id
            };

            _subjectService.SaveSubject(subject);

            return Json(new {error = false, msg = "Save Successful!"});
        }
        #endregion
    }
}