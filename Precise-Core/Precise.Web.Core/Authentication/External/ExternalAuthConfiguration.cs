using Abp.Dependency;
using System.Collections.Generic;

namespace Precise.Web.Authentication.External
{
    public class ExternalAuthConfiguration : IExternalAuthConfiguration, ISingletonDependency
    {
        public List<ExternalLoginProviderInfo> Providers { get; }

        public ExternalAuthConfiguration()
        {
            this.Providers = new List<ExternalLoginProviderInfo>();
        }
    }
}
