using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Abp;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using Abp.Threading;
using Microsoft.IdentityModel.Tokens;
using Precise.Authorization.Users;

namespace Precise.Web.Authentication.JwtBearer
{
    public class PreciseJwtSecurityTokenHandler : ISecurityTokenValidator
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public PreciseJwtSecurityTokenHandler()
        {
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public bool CanValidateToken => true;

        public int MaximumTokenSizeInBytes { get; set; } = TokenValidationParameters.DefaultMaximumTokenSizeInBytes;

        public bool CanReadToken(string securityToken)
        {
            return _tokenHandler.CanReadToken(securityToken);
        }

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            var cacheManager = IocManager.Instance.Resolve<ICacheManager>();

            var principal = _tokenHandler.ValidateToken(securityToken, validationParameters, out validatedToken);

            var userIdentifierString = principal.Claims.First(c => c.Type == AppConsts.UserIdentifier);
            var tokenValidityKeyInClaims = principal.Claims.First(c => c.Type == AppConsts.TokenValidityKey);

            var tokenValidityKeyInCache = cacheManager
                .GetCache(AppConsts.TokenValidityKey)
                .GetOrDefault(tokenValidityKeyInClaims.Value);

            if (tokenValidityKeyInCache != null)
            {
                return principal;
            }

            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = unitOfWorkManager.Object.Begin())
                {
                    var userIdentifier = UserIdentifier.Parse(userIdentifierString.Value);

                    using (unitOfWorkManager.Object.Current.SetTenantId(userIdentifier.TenantId))
                    {
                        using (var userManager = IocManager.Instance.ResolveAsDisposable<UserManager>())
                        {
                            var userManagerObject = userManager.Object;
                            var user = userManagerObject.GetUser(userIdentifier);
                            var isValidityKetValid = AsyncHelper.RunSync(() => userManagerObject.IsTokenValidityKeyValidAsync(user, tokenValidityKeyInClaims.Value));
                            uow.Complete();

                            if (isValidityKetValid)
                            {
                                cacheManager
                                    .GetCache(AppConsts.TokenValidityKey)
                                    .Set(tokenValidityKeyInClaims.Value, "");

                                return principal;
                            }
                        }
                    }

                    throw new SecurityTokenException("invalid");
                }
            }
        }
    }
}