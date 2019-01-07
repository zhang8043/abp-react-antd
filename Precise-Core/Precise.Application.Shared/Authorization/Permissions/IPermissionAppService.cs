using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Precise.Authorization.Permissions.Dto;

namespace Precise.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
