using Abp.Domain.Services;

namespace Precise
{
    public abstract class PreciseDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected PreciseDomainServiceBase()
        {
            LocalizationSourceName = PreciseConsts.LocalizationSourceName;
        }
    }
}
