using Microsoft.AspNetCore.Mvc;
using Store.Domain.Exceptions.BadRequest;
using Store.Domain.Exceptions.NotFound;
using Store.Domain.Exceptions.UnAuthorized;
using Store.Shared.ErrorModels;

namespace Store.Web.Middlewares
{
    public class GlobalErrorHandlingMiddlewares(RequestDelegate _next, ILogger<GlobalErrorHandlingMiddlewares> _logger)
    {
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
                if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound && !httpContext.Response.HasStarted)
                {
                    httpContext.Response.ContentType = "application/json";
                    var problem = new ProblemDetails()
                    {
                        Title = "EndPoint.NotFound",
                        Detail = $"EndPoint with path {httpContext.Request.Path} is not found",
                        Status = StatusCodes.Status404NotFound,
                        Instance = httpContext.Request.Path,
                        Extensions = { ["traceId"] = httpContext.TraceIdentifier }
                    };
                    await httpContext.Response.WriteAsJsonAsync(problem);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong");

                if (httpContext.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the error handler will not be executed.");
                    throw;
                }

                var problem = new ProblemDetails()
                {
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occurred",
                    Status = StatusCodes.Status500InternalServerError,
                    Instance = httpContext.Request.Path,
                    Extensions = { ["traceId"] = httpContext.TraceIdentifier }
                };
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsJsonAsync(problem);
            }
        }
    }
}
