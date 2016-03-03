using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using HomeRoom.Authorization;
using HomeRoom.MultiTenancy;

namespace HomeRoom.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Tenants)]
    public class TenantsController : HomeRoomControllerBase
    {
        private readonly ITenantAppService _tenantAppService;

        public TenantsController(ITenantAppService tenantAppService)
        {
            _tenantAppService = tenantAppService;
        }

        public ActionResult Index()
        {
            var output = _tenantAppService.GetTenants();
            return View(output);
        }
    }
}