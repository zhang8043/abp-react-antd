using System.Threading.Tasks;
using Abp.Application.Services;
using Precise.Sessions.Dto;

namespace Precise.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
