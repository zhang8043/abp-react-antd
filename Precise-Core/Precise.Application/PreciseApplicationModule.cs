using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Precise.Authorization;

namespace Precise
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(PreciseCoreModule)
        )]
    public class PreciseApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PreciseApplicationModule).GetAssembly());
        }
    }
}