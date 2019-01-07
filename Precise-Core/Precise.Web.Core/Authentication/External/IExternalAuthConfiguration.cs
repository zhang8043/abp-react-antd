using System;
using System.Collections.Generic;
using System.Text;

namespace Precise.Web.Authentication.External
{
    public interface IExternalAuthConfiguration
    {
        List<ExternalLoginProviderInfo> Providers { get; }
    }
}
