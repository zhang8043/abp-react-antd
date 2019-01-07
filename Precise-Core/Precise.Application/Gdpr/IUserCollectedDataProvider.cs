using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Precise.Dto;

namespace Precise.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
