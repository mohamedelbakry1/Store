using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Store.Services.Abstractions;
using Store.Services.Abstractions.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Presentation.Attributes
{
    public class RedisCacheAttribute : ActionFilterAttribute
    {
        private readonly int _durationInDays;
        public RedisCacheAttribute(int durationInDays = 2)
        {
            _durationInDays = durationInDays;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            var cacheKey = CreateCacheKey(context.HttpContext.Request);

            var data = await cacheService.GetDataAsync(cacheKey);

            if (!string.IsNullOrEmpty(data))
            {
                context.Result = new ContentResult()
                {
                    Content = data,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }

            var executedContext = await next.Invoke();

            if (executedContext.Result is OkObjectResult { Value: not null } ok)
            {
                await cacheService.SetDataAsync(cacheKey, ok.Value, TimeSpan.FromDays(_durationInDays));
            }
        }


        private string CreateCacheKey(HttpRequest request)
        {
            var key = new StringBuilder();
            key.Append(request.Path);
            if (request.Query.Any())
            {
                key.Append("?");
                foreach (var (k, v) in request.Query.OrderBy(x => x.Key))
                {
                    key.Append(k).Append("=").Append(v).Append("&");
                }
            }
            return key.ToString();
        }
    }
}
