using System;
using Abp.Runtime.Caching;

namespace Precise.Storage
{
    public class TempFileCacheManager : ITempFileCacheManager
    {
        public const string TempFileCacheName = "TempFileCacheName";

        private readonly ICacheManager _cacheManager;

        public TempFileCacheManager(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public void SetFile(string token, byte[] content)
        {
            _cacheManager.GetCache(TempFileCacheName).Set(token, content, new TimeSpan(0, 0, 1, 0)); // expire time is 1 min by default
        }

        public byte[] GetFile(string token)
        {
            return _cacheManager.GetCache(TempFileCacheName).Get(token, ep => ep) as byte[];
        }
    }
}