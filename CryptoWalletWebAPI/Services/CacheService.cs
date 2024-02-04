using CryptoWalletWebAPI.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace CryptoWalletWebAPI.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache cache;

        public CacheService(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public bool TryGetValue<T>(string cacheKey, out T value)
        {
            return this.cache.TryGetValue(cacheKey, out value);
        }

        public void Set<T>(string cacheKey, T value, TimeSpan? absoluteExpireTime = null)
        {
            if (absoluteExpireTime.HasValue)
            {
                this.cache.Set(cacheKey, value, absoluteExpireTime.Value);
            }
            else
            {
                this.cache.Set(cacheKey, value);
            }
        }

        public void Remove(string cacheKey)
        {
            this.cache.Remove(cacheKey);
        }
    }
}
