using System.Threading.Tasks;
using Abp.Domain.Repositories;

namespace Precise.MultiTenancy.Payments
{
    public interface ISubscriptionPaymentRepository : IRepository<SubscriptionPayment, long>
    {
        Task<SubscriptionPayment> UpdateByGatewayAndPaymentIdAsync(SubscriptionPaymentGatewayType gateway, string paymentId, int? tenantId, SubscriptionPaymentStatus status);

        Task<SubscriptionPayment> GetByGatewayAndPaymentIdAsync(SubscriptionPaymentGatewayType gateway, string paymentId);
    }
}
