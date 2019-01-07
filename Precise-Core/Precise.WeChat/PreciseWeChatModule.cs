using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Precise
{
    [DependsOn(
       typeof(PreciseCoreModule),
       typeof(AbpAutoMapperModule))]
    public class PreciseWeChatModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PreciseWeChatModule).GetAssembly());
        }

        public override void PostInitialize()
        {

        }

        public override void Shutdown()
        {

        }
    }
}
