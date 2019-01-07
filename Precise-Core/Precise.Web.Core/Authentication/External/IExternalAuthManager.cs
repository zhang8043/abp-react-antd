using System.Threading.Tasks;

namespace Precise.Web.Authentication.External
{
    public interface IExternalAuthManager
    {
        Task<bool> IsValidUser(string provider, string providerKey, string providerAccessCode);
        Task<ExternalAuthUserInfo> GetUserInfo(string provider, string accessCode);
    }
}
