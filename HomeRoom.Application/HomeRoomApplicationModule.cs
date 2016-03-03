using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;

namespace HomeRoom
{
    [DependsOn(typeof(HomeRoomCoreModule), typeof(AbpAutoMapperModule))]
    public class HomeRoomApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
