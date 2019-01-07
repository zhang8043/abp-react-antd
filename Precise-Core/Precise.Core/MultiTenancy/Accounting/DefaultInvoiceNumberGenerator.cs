using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Timing;
using Microsoft.EntityFrameworkCore;

namespace Precise.MultiTenancy.Accounting
{
    public class DefaultInvoiceNumberGenerator : IInvoiceNumberGenerator
    {
        private readonly IRepository<Invoice> _invoiceRepository;

        public DefaultInvoiceNumberGenerator(IRepository<Invoice> invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        [UnitOfWork]
        public async Task<string> GetNewInvoiceNumber()
        {
            var lastInvoice = await _invoiceRepository.GetAll().OrderByDescending(i => i.Id).FirstOrDefaultAsync();
            if (lastInvoice == null)
            {
                return Clock.Now.Year + "" + (Clock.Now.Month).ToString("00") + "00001";
            }

            var year = Convert.ToInt32(lastInvoice.InvoiceNo.Substring(0, 4));
            var month = Convert.ToInt32(lastInvoice.InvoiceNo.Substring(4, 2));

            var invoiceNumberToIncrease = lastInvoice.InvoiceNo.Substring(6, lastInvoice.InvoiceNo.Length - 6);
            if (year != Clock.Now.Year || month != Clock.Now.Month)
            {
                invoiceNumberToIncrease = "0";
            }

            var invoiceNumberPostfix = Convert.ToInt32(invoiceNumberToIncrease) + 1;
            return Clock.Now.Year + (Clock.Now.Month).ToString("00") + invoiceNumberPostfix.ToString("00000");
        }
    }
}
