using Abp.Auditing;
using Microsoft.AspNetCore.Mvc;

namespace Precise.Web.Controllers
{
    public class HomeController : PreciseControllerBase
    {
        [DisableAuditing]
        public IActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}
