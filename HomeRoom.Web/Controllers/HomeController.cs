﻿using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;

namespace HomeRoom.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : HomeRoomControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}