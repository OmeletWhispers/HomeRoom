using System.Web.Mvc;

namespace HomeRoom.Web.Controllers
{
    public class AboutController : HomeRoomControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}