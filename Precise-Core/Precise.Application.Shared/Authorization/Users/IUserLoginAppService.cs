using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Precise.Authorization.Users.Dto;

namespace Precise.Authorization.Users
{
    public interface IUserLoginAppService : IApplicationService
    {
        Task<ListResultDto<UserLoginAttemptDto>> GetRecentUserLoginAttempts();
    }
}
