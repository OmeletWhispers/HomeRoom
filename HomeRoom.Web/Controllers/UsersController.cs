using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HomeRoom.Datatables;
using HomeRoom.DataTableDto;
using HomeRoom.Users;
using Microsoft.Owin.Security;
using Web.Extensions;

namespace HomeRoom.Web.Controllers
{
    public class UsersController : HomeRoomControllerBase
    {
        #region Private Fields

        private readonly UserManager _userManager;
        private readonly IUserAppService _userService; 

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        #endregion

        #region Constructors
        public UsersController(UserManager userManager, IUserAppService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index()
        {
            return View("Users");
        }

        public ActionResult GetDataTable([ModelBinder(typeof(ModelBinderDataTableExtension))] IDataTableRequest request)
        {
            request.Length = request.Length < HomeRoomConsts.MinLength ? HomeRoomConsts.MinLength : request.Length;

            var sortedColumns = request.Columns.Where(x => x.IsOrdered).OrderBy(x => x.OrderNumber);

            var dataTableRequest = new DataTableRequestDto(request.Draw, request.Start, request.Length, sortedColumns.FirstOrDefault(), request.Search);

            var users = _userService.GetAllUsers(dataTableRequest);
            
            return Json(users.ToDataTableResponse(), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}