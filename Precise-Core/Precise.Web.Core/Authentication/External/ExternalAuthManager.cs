using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;

namespace Precise.Web.Authentication.External
{
    public class ExternalAuthManager : IExternalAuthManager, ITransientDependency
    {
        public ExternalAuthManager(IIocResolver iocResolver, IExternalAuthConfiguration externalAuthConfiguration)
        {
            this._iocResolver = iocResolver;
            this._externalAuthConfiguration = externalAuthConfiguration;
        }

        public Task<bool> IsValidUser(string provider, string providerKey, string providerAccessCode)
        {
            Task<bool> result;
            using (IDisposableDependencyObjectWrapper<IExternalAuthProviderApi> disposableDependencyObjectWrapper = this.CreateProviderApi(provider))
            {
                result = disposableDependencyObjectWrapper.Object.IsValidUser(providerKey, providerAccessCode);
            }
            return result;
        }

        public Task<ExternalAuthUserInfo> GetUserInfo(string provider, string accessCode)
        {
            Task<ExternalAuthUserInfo> userInfo;
            using (IDisposableDependencyObjectWrapper<IExternalAuthProviderApi> disposableDependencyObjectWrapper = this.CreateProviderApi(provider))
            {
                userInfo = disposableDependencyObjectWrapper.Object.GetUserInfo(accessCode);
            }
            return userInfo;
        }

        public IDisposableDependencyObjectWrapper<IExternalAuthProviderApi> CreateProviderApi(string provider)
        {
            ExternalLoginProviderInfo externalLoginProviderInfo = this._externalAuthConfiguration.Providers.FirstOrDefault((ExternalLoginProviderInfo p) => p.Name == provider);
            if (externalLoginProviderInfo == null)
            {
                throw new Exception("Unknown external auth provider: " + provider);
            }
            IDisposableDependencyObjectWrapper<IExternalAuthProviderApi> disposableDependencyObjectWrapper = IocResolverExtensions.ResolveAsDisposable<IExternalAuthProviderApi>(this._iocResolver, externalLoginProviderInfo.ProviderApiType);
            disposableDependencyObjectWrapper.Object.Initialize(externalLoginProviderInfo);
            return disposableDependencyObjectWrapper;
        }

        private readonly IIocResolver _iocResolver;

        private readonly IExternalAuthConfiguration _externalAuthConfiguration;
    }
}
