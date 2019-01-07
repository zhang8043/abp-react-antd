using System.IO;
using Abp.Dependency;
using Microsoft.Extensions.Configuration;

namespace Precise.Configuration
{
    /* This service is replaced in Web layer and Test project separately */
    public class DefaultAppConfigurationAccessor : IAppConfigurationAccessor, ISingletonDependency
    {
        public IConfigurationRoot Configuration { get; }
        
        public DefaultAppConfigurationAccessor()
        {
            Configuration = AppConfigurations.Get(Directory.GetCurrentDirectory());
        }
    }
}