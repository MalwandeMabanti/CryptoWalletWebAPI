namespace CryptoWalletWebAPI.Interfaces
{
    public interface ICacheService
    {
        bool TryGetValue<T>(string cacheKey, out T value);
        void Set<T>(string cacheKey, T value, TimeSpan? absoluteExpireTime = null);
        void Remove(string cacheKey);
    }
}
