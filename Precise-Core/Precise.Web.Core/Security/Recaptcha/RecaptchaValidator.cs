using System;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Http;
using Precise.Security.Recaptcha;
using PaulMiami.AspNetCore.Mvc.Recaptcha;

namespace Precise.Web.Security.Recaptcha
{
    public class RecaptchaValidator : PreciseServiceBase, IRecaptchaValidator, ITransientDependency
    {
        public const string RecaptchaResponseKey = "g-recaptcha-response";

        private readonly IRecaptchaValidationService _recaptchaValidationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RecaptchaValidator(IRecaptchaValidationService recaptchaValidationService, IHttpContextAccessor httpContextAccessor)
        {
            _recaptchaValidationService = recaptchaValidationService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task ValidateAsync(string captchaResponse)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new Exception("RecaptchaValidator应该在有效的HTTP上下文中使用!");
            }

            if (captchaResponse.IsNullOrEmpty())
            {
                throw new UserFriendlyException(L("CaptchaCanNotBeEmpty"));
            }

            try
            {
                await _recaptchaValidationService.ValidateResponseAsync(
                    captchaResponse,
                    _httpContextAccessor.HttpContext.Connection?.RemoteIpAddress?.ToString()
                );
            }
            catch (RecaptchaValidationException)
            {
                throw new UserFriendlyException(L("IncorrectCaptchaAnswer"));
            }
        }
    }
}
