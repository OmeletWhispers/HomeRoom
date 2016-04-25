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

namespace HomeRoom.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ParentController : HomeRoomControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly IClassService _classService;

        public ParentController(IUserAppService userAppService, IClassService classService)
        {
            _userAppService = userAppService;
            _classService = classService;
        }

        public ActionResult GetDataTable([ModelBinder(typeof(ModelBinderDataTableExtension))] IDataTableRequest request)
        {
            request.Length = request.Length < HomeRoomConsts.MinLength ? HomeRoomConsts.MinLength : request.Length;

            var sortedColumns = request.Columns.Where(x => x.IsOrdered).OrderBy(x => x.OrderNumber);

            var dataTableRequest = new DataTableRequestDto(request.Draw, request.Start, request.Length, sortedColumns.FirstOrDefault(), request.Search);

            var users = _userAppService.GetParentStudents(dataTableRequest);

            return Json(users.ToDataTableResponse(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult StudentClasses(int id)
        {
            var studentClasses = _classService.GetStudentClasses(id);

            return PartialView("Tables/_ParentStudentClassesTable", studentClasses);
        }
    }
}