using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Precise.Authorization;
using Precise.MultiTenancy;
using Precise.Net.MimeTypes;
using Precise.Storage;
using Precise.Web.Helpers;

namespace Precise.Web.Controllers
{
    [AbpMvcAuthorize]
    public class TenantCustomizationController : PreciseControllerBase
    {
        private readonly TenantManager _tenantManager;
        private readonly IBinaryObjectManager _binaryObjectManager;

        public TenantCustomizationController(
            TenantManager tenantManager,
            IBinaryObjectManager binaryObjectManager)
        {
            _tenantManager = tenantManager;
            _binaryObjectManager = binaryObjectManager;
        }

        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Tenant_Settings)]
        public async Task<JsonResult> UploadLogo()
        {
            try
            {
                var logoFile = Request.Form.Files.First();

                //Check input
                if (logoFile == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (logoFile.Length > 30720) //30KB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                using (var stream = logoFile.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var imageFormat = ImageFormatHelper.GetRawImageFormat(fileBytes);
                if (!imageFormat.IsIn(ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif))
                {
                    throw new UserFriendlyException("File_Invalid_Type_Error");
                }

                var logoObject = new BinaryObject(AbpSession.GetTenantId(), fileBytes);
                await _binaryObjectManager.SaveAsync(logoObject);

                var tenant = await _tenantManager.GetByIdAsync(AbpSession.GetTenantId());
                tenant.LogoId = logoObject.Id;
                tenant.LogoFileType = logoFile.ContentType;

                return Json(new AjaxResponse(new { id = logoObject.Id, fileType = tenant.LogoFileType }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Tenant_Settings)]
        public async Task<JsonResult> UploadCustomCss()
        {
            try
            {
                var cssFile = Request.Form.Files.First();

                //Check input
                if (cssFile == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (cssFile.Length > 1048576) //1MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                using (var stream = cssFile.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var cssFileObject = new BinaryObject(AbpSession.GetTenantId(), fileBytes);
                await _binaryObjectManager.SaveAsync(cssFileObject);

                var tenant = await _tenantManager.GetByIdAsync(AbpSession.GetTenantId());
                tenant.CustomCssId = cssFileObject.Id;

                return Json(new AjaxResponse(new { id = cssFileObject.Id }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> GetLogo()
        {
            var tenant = await _tenantManager.GetByIdAsync(AbpSession.GetTenantId());
            if (!tenant.HasLogo())
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            var logoObject = await _binaryObjectManager.GetOrNullAsync(tenant.LogoId.Value);
            if (logoObject == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            return File(logoObject.Bytes, tenant.LogoFileType);
        }

        [AllowAnonymous]
        public async Task<ActionResult> GetTenantLogo(string skin, int? tenantId)
        {
            var defaultLogo = "/Common/Images/app-logo-on-" + skin + ".svg";

            if (tenantId == null)
            {
                return File(defaultLogo, MimeTypeNames.ImagePng);
            }

            var tenant = await _tenantManager.GetByIdAsync(tenantId.Value);
            if (!tenant.HasLogo())
            {
                return File(defaultLogo, MimeTypeNames.ImagePng);
            }

            using (CurrentUnitOfWork.SetTenantId(tenantId.Value))
            {
                var logoObject = await _binaryObjectManager.GetOrNullAsync(tenant.LogoId.Value);
                if (logoObject == null)
                {
                    return File(defaultLogo, MimeTypeNames.ImagePng);
                }

                return File(logoObject.Bytes, tenant.LogoFileType);
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> GetCustomCss()
        {
            var tenant = await _tenantManager.GetByIdAsync(AbpSession.GetTenantId());
            if (!tenant.CustomCssId.HasValue)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            var cssFileObject = await _binaryObjectManager.GetOrNullAsync(tenant.CustomCssId.Value);
            if (cssFileObject == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            return File(cssFileObject.Bytes, MimeTypeNames.TextCss);
        }
    }
}