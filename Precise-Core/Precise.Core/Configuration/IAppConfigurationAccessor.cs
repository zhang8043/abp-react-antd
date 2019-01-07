using Microsoft.Extensions.Configuration;

namespace Precise.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
