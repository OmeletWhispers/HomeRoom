using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using Abp.Zero.EntityFramework;
using HomeRoom.EntityFramework;

namespace HomeRoom
{
    [DependsOn(typeof(AbpZeroEntityFrameworkModule), typeof(HomeRoomCoreModule))]
    public class HomeRoomDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
