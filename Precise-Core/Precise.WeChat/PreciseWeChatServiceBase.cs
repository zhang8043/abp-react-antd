using Abp;

namespace Precise
{
    /// <summary>
    /// 该应用程序中的服务的基类。
    /// </summary>
    public abstract class PreciseWeChatServiceBase : AbpServiceBase
    {
        protected PreciseWeChatServiceBase()
        {
            LocalizationSourceName = PreciseConsts.LocalizationSourceName;
        }
    }
}
