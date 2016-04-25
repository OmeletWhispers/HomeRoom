using System;
using System.Web.Mvc;
using Abp.Runtime.Session;
using Abp.Web.Mvc.Authorization;
using HomeRoom.Enumerations;
using HomeRoom.Users;
using Microsoft.AspNet.Identity;

namespace HomeRoom.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : HomeRoomControllerBase
    {
        private readonly UserManager _userManager;

        public HomeController(UserManager userManager)
        {
            _userManager = userManager;
        }

        public ActionResult Index()
        {
            var userId = AbpSession.GetUserId();

            var user = _userManager.FindById(userId);

            switch (user.AccountType)
            {
                case AccountType.Teacher:
                    return RedirectToAction("Index","Class");
                case AccountType.Student:
                    
                    break;
                case AccountType.Parent:
                    return RedirectToAction("Index","Parent");
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return View();
        }
    }
}