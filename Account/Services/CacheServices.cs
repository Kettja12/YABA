using Microsoft.Extensions.Caching.Memory;
namespace Account.Services;
public class CacheServices
{
    private readonly IMemoryCache memoryCache;
    
    public CacheServices(IMemoryCache memoryCache)
    {
        this.memoryCache = memoryCache;
    }

    public T? GetCacheItem<T>(string prefix, string sessionId)
    {
        string key = $"{prefix}-{sessionId}";
        if (memoryCache.TryGetValue(key, out T? t))
        {
            return t;
        };
        return default;

    }
    public string SetCacheItem<T>(string prefix, string token, T item)
    {
        try
        {
            string key = $"{prefix}-{token}";
            var cacheExpiryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddDays(1),
                Priority = CacheItemPriority.High,
            };
            memoryCache.Set(key, item, cacheExpiryOptions);
            return token;
        }
        catch
        {
            return "";
        }
    }
    public bool DeleteCacheItem(string prefix, string token)
    {
        try
        {
            string key = $"{prefix}-{token}";
            memoryCache.Remove(key);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
