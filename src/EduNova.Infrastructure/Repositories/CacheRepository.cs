using EduNova.Application.Contracts;
using StackExchange.Redis;

namespace EduNova.Infrastructure.Repositories;

public class CacheRepository(IConnectionMultiplexer connection) : ICacheRepository
{
    private readonly IDatabase _database = connection.GetDatabase();

    public async Task SetDataAsync(string cacheKey, string cacheValue, TimeSpan ttl)
    {
         await _database.StringSetAsync(cacheKey, cacheValue, ttl);
    }

    public async Task<string?> GetDataAsync(string cacheKey)
    {
        var cacheValue = await _database.StringGetAsync(cacheKey);
        return cacheValue.IsNullOrEmpty ? null : cacheValue.ToString();
    }
}