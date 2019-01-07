using System.Collections.Generic;
using Abp.AutoMapper;
using Precise.Web.Authentication.External;

namespace Precise.Web.Models.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }

        public string ClientId { get; set; }

        public Dictionary<string, string> AdditionalParams { get; set; }

    }
}
