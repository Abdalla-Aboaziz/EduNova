using System.Text;
using EduNova.Application.Common.Caching;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EduNova.API.Attributes;

public class RedisCacheAttribute(int duration = 5) : ActionFilterAttribute
{
    private readonly int _duration = duration;

    public override async Task OnActionExecutionAsync(ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        
        // get cache service from di 
        var cacheService = context.HttpContext.RequestServices
                           .GetRequiredService<ICacheService>();
        
        // create cacheKey
        var cacheKey = CreateCacheKey(context.HttpContext.Request);
        
        // check if data exist
        var cacheData = await cacheService.GetDataAsync(cacheKey);
        if (cacheData is not null)
        {
            context.Result = new ContentResult()
            {
                Content = cacheData,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK

            };
            
            return;
        }

        var nextContext = await next.Invoke();
        if (nextContext.Result is OkObjectResult result)
        {
         await   cacheService.SetDataAsync(cacheKey,
             result.Value! ,
             TimeSpan.FromMinutes(_duration));
        }

    }
    
    // create cacheKey
    private string CreateCacheKey(HttpRequest request)
    {
        var key = new StringBuilder();
        key.Append(request.Path);

        foreach (var item in request.Query.OrderBy(x => x.Key))
        {
            key.Append($"{item.Key}-{item.Value}");
        }
        return key.ToString();
    }
}