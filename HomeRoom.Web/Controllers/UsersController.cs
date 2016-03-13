using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly ITeacherService _teacherService;

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        #endregion

        #region Constructors
        public UsersController(UserManager userManager, IUserAppService userService, IUnitOfWorkManager unitOfWorkManager, TenantManager tenantManager, ITeacherService teacherService)
        {
            _userManager = userManager;
            _userService = userService;
            _unitOfWorkManager = unitOfWorkManager;
            _tenantManager = tenantManager;
            _teacherService = teacherService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index()
        {
            return View("Users");
        }

        [HttpGet]
        public async Task<ActionResult> Users(long? userId)
        {
            var model = new UserViewModel();
            // edit user
            if (userId.HasValue)
            {
                var user = await _userManager.GetUserByIdAsync(userId.Value);
                model.Id = userId.Value;
                model.FirstName = user.Name;
                model.LastName = user.Surname;
                model.Email = user.EmailAddress;
                model.Gender = user.Gender;
                model.AccountType = user.AccountType;
            }

            return PartialView("Forms/_CreateUserForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Users(UserViewModel userViewModel)
        {

            // checks to see if their were an model errors 
            CheckModelState();


            var tenant = await _tenantManager.FindByTenancyNameAsync(Tenant.DefaultTenantName);

            var user = new User
            {
                Id = userViewModel.Id,
                TenantId = tenant.Id,
                AccountType = userViewModel.AccountType,
                EmailAddress = userViewModel.Email.ToLower(),
                Name = userViewModel.FirstName,
                Surname = userViewModel.LastName,
                UserName = userViewModel.Email.ToLower(),
                Gender = userViewModel.Gender,
                Password = new PasswordHasher().HashPassword(HomeRoom.Users.User.DefaultPassword),
                IsActive = true,
            };

            if (userViewModel.Id != 0)
            {
                CheckErrors(await _userManager.UpdateAsync(user));

                switch (user.AccountType)
                {
                    case AccountType.Teacher:
                        // only insert this teacher if they were not already a teacher
                        if (!_teacherService.IsUserTeacher(user.Id))
                        {
                            await _teacherService.InsertTeacher(user.Id);
                        }
                        break;
                    case AccountType.Student:
                        break;
                    case AccountType.Parent:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                return Json(new {msg = "Save Successful!"});
            }

            CheckErrors(await _userManager.CreateAsync(user));

            return Json(new {msg = "Save Successful!"});
        }

        public ActionResult GetDataTable([ModelBinder(typeof (ModelBinderDataTableExtension))] IDataTableRequest request)
        {
            request.Length = request.Length < HomeRoomConsts.MinLength ? HomeRoomConsts.MinLength : request.Length;

            var sortedColumns = request.Columns.Where(x => x.IsOrdered).OrderBy(x => x.OrderNumber);

            var dataTableRequest = new DataTableRequestDto(request.Draw, request.Start, request.Length, sortedColumns.FirstOrDefault(), request.Search);

            var users = _userService.GetAllUsers(dataTableRequest);

            return Json(users.ToDataTableResponse(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteUser(long userId)
        {
            var user = _userManager.GetUserByIdAsync(userId);

            await _userManager.DeleteAsync(await user);

            var message = user.Result.Name + " has been deleted.";
            return Json(new {msg = message}, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}