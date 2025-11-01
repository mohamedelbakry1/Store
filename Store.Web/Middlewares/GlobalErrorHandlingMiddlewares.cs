using Store.Domain.Exceptions.BadRequest;
using Store.Domain.Exceptions.NotFound;
using Store.Shared.ErrorModels;

namespace Store.Web.Middlewares
{
    public class GlobalErrorHandlingMiddlewares
    {
        private readonly RequestDelegate _next;

        public GlobalErrorHandlingMiddlewares(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                if(context.Response.StatusCode == StatusCodes.Status404NotFound) // routing middleware
                {
                    context.Response.ContentType = "application/json";
                    var response = new ErrorDetails()
                    {
                        StatusCode = context.Response.StatusCode,
                        ErrorMessage = $"End Point {context.Request.Path} was Not Found !!"
                    };

                    await context.Response.WriteAsJsonAsync(response);
                }
            }
            catch (Exception ex)
            {
                // logic

                // 1. Set Status Code of Response
                context.Response.StatusCode = ex switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    BadRequestException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                };

                // 2. Set Content Type of Response
                context.Response.ContentType = "application/json";

                // 3. Set Body of Response

                var response = new ErrorDetails()
                {
                    StatusCode = context.Response.StatusCode,
                    ErrorMessage = ex.Message
                };

                // return Response
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
