using System.Text;
using Abp.Extensions;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Precise.Web.Swagger
{
    public static class SwaggerExtensions
    {
        /// <summary>
        /// Injects ABP base URI into the index.html page
        /// </summary>
        /// <param name="options"></param>
        /// <param name="pathBase">base path (URL) to application API</param>
        public static void InjectBaseUrl(this SwaggerUIOptions options, string pathBase)
        {
            pathBase = pathBase.EnsureEndsWith('/');

            options.HeadContent = new StringBuilder(options.HeadContent)
                .AppendLine($"<script> var abp = abp || {{}}; abp.appPath = abp.appPath || '{pathBase}'; </script>")
                .ToString();
        }
    }
}
