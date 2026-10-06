namespace EduNova.Application.Contracts;

public interface ICacheRepository
{
    // set data in cache 
    Task SetDataAsync(string cacheKey, string cacheValue, TimeSpan ttl);
    
    // get data from cache 
    Task<string?>  GetDataAsync(string cacheKey); 
    
}