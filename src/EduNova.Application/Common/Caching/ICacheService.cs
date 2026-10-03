namespace EduNova.Application.Common.Caching;

public interface ICacheService
{
    // get data from cache
    Task<string?> GetDataAsync(string cacheKey);

    // set data in cache
    Task SetDataAsync(string cacheKey, object cacheValue, TimeSpan ttl);
}