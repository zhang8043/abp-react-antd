using System.Threading.Tasks;
using Abp.Application.Services;
using Precise.Editions.Dto;
using Precise.MultiTenancy.Dto;

namespace Precise.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}