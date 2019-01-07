using System.Threading.Tasks;

namespace Precise.Identity
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}