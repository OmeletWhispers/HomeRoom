using System;
using System.Diagnostics;
using Abp.Web;
using Castle.Facilities.Logging;

namespace HomeRoom.Web
{
    public class MvcApplication : AbpWebApplication
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            AbpBootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(f => f.UseLog4Net().WithConfig("log4net.config"));
            base.Application_Start(sender, e);
        }

        protected override void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            var message = exception.Message;
            var inner = exception.InnerException;

            base.Application_Error(sender, e);
        }
    }
}
