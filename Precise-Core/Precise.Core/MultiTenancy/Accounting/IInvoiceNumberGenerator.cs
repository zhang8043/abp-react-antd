using System.Threading.Tasks;
using Abp.Dependency;

namespace Precise.MultiTenancy.Accounting
{
    public interface IInvoiceNumberGenerator : ITransientDependency
    {
        Task<string> GetNewInvoiceNumber();
    }
}