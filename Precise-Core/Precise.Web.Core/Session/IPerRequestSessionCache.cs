using System.Threading.Tasks;
using Precise.Sessions.Dto;

namespace Precise.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
