using System.Text.Json;
using EduNova.Application.Common.Caching;
using EduNova.Application.Contracts;

namespace EduNova.Infrastructure.Services;

public class CacheService(ICacheRepository cacheRepository) : ICacheService
{
    public async Task<string?> GetDataAsync(string cacheKey)
    {
        return await cacheRepository.GetDataAsync(cacheKey);
    }

    public Task SetDataAsync(string cacheKey, object cacheValue, TimeSpan ttl)
    {
        var cacheData = JsonSerializer.Serialize(cacheValue); // convert data from string to json 
        return cacheRepository.SetDataAsync(cacheKey, cacheData, ttl);
    }
}