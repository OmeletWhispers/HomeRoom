using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HomeRoom.Datatables;
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

            var users = _userService.GetAllUsers(request.Start, request.Length, request.ColumnData.GetColumnsSorted().FirstOrDefault(), request.Search);

            var userTable = users.Select(x => new
            {
                x.Id,
                FirstName = x.Name,
                LastName = x.Surname,
                Email = x.EmailAddress
            });

            return Json(new DataTableResponse(request.Draw, userTable, users.Count, users.Count), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}