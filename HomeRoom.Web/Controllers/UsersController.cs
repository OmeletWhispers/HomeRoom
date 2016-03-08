using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Domain.Uow;
using Abp.UI;
using Abp.Web.Mvc.Models;
using HomeRoom.Datatables;
using HomeRoom.DataTableDto;
using HomeRoom.Enumerations;
using HomeRoom.MultiTenancy;
using HomeRoom.Users;
using HomeRoom.Web.Models.Membership;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Web.Extensions;

namespace HomeRoom.Web.Controllers
{
    public class UsersController : HomeRoomControllerBase
    {
        #region Private Fields

        private readonly UserManager _userManager;
        private readonly IUserAppService _userService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly TenantManager _tenantManager;

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        #endregion

        #region Constructors
        public UsersController(UserManager userManager, IUserAppService userService, IUnitOfWorkManager unitOfWorkManager, TenantManager tenantManager)
        {
            _userManager = userManager;
            _userService = userService;
            _unitOfWorkManager = unitOfWorkManager;
            _tenantManager = tenantManager;
        }

        #endregion

        #region Public Methods

        public ActionResult Index()
        {
            return View("Users");
        }

        [HttpGet]
        public ActionResult Users(int? userId)
        {
            return PartialView("Forms/_CreateUserForm", new UserViewModel());
        }

        [HttpPost]
        [UnitOfWork]
        public JsonResult Users(UserViewModel userViewModel)
        {

            // checks to see if their were an model errors 
            CheckModelState();


            var user = new User
            {
                AccountType = userViewModel.AccountType,
                EmailAddress = userViewModel.Email.ToLower(),
                Name = userViewModel.FirstName,
                Surname = userViewModel.LastName,
                UserName = userViewModel.Email.ToLower(),
                Gender = userViewModel.Gender,
                Password = new PasswordHasher().HashPassword(HomeRoom.Users.User.DefaultPassword),
                IsActive = true,
            };

            CheckErrors(_userManager.Create(user));

            return Json(new { msg = "Save Successful!" });


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