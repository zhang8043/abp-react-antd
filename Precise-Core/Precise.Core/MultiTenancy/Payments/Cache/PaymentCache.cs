using Abp.Dependency;
using Abp.Runtime.Caching;

namespace Precise.MultiTenancy.Payments.Cache
{
    /// <summary>
    /// This cache is used to temporarily store "paid" information while tenant registration form is being filled.
    /// </summary>
    public class PaymentCache : IPaymentCache, ISingletonDependency
    {
        private readonly ICacheManager _cacheManager;

        public PaymentCache(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public virtual PaymentCacheItem GetCacheItemOrNull(SubscriptionPaymentGatewayType gateway, string paymentId)
        {
            return _cacheManager
                .GetCache(PaymentCacheItem.CacheName)
                .GetOrDefault<string, PaymentCacheItem>(GetCacheKey(gateway, paymentId));
        }

        public void AddCacheItem(PaymentCacheItem item)
        {
            _cacheManager
                .GetCache(PaymentCacheItem.CacheName)
                .Set(GetCacheKey(item.GateWay, item.PaymentId), item);
        }

        public void RemoveCacheItem(SubscriptionPaymentGatewayType gateway, string paymentId)
        {
            var key = GetCacheKey(gateway, paymentId);
            var cacheItem = GetCacheItemOrNull(gateway, paymentId);
            if (cacheItem == null)
            {
                return;
            }

            _cacheManager.GetCache(PaymentCacheItem.CacheName).Remove(key);
        }

        private static string GetCacheKey(SubscriptionPaymentGatewayType gateway, string paymentId)
        {
            return gateway + "_" + paymentId;
        }
    }
}