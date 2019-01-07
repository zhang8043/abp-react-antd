using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Precise.MultiTenancy.Accounting.Dto;

namespace Precise.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
