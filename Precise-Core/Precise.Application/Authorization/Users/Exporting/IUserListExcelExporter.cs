using System.Collections.Generic;
using Precise.Authorization.Users.Dto;
using Precise.Dto;

namespace Precise.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}