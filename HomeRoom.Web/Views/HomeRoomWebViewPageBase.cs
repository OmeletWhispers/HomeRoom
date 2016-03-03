using Abp.Web.Mvc.Views;

namespace HomeRoom.Web.Views
{
    public abstract class HomeRoomWebViewPageBase : HomeRoomWebViewPageBase<dynamic>
    {

    }

    public abstract class HomeRoomWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected HomeRoomWebViewPageBase()
        {
            LocalizationSourceName = HomeRoomConsts.LocalizationSourceName;
        }
    }
}