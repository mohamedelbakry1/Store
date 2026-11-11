using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Store.Presentation
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBaseController : ControllerBase
    {
        public static ActionResult ToActionResult(Result result)
        {
            if (result.IsSuccess)
                return new OkResult();
            else
                return ToProblem(result.Errors);
        }

        public static ActionResult<T> ToActionResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
                return new OkObjectResult(result.Value);
            else
                return ToProblem(result.Errors);
        }

        private static OkObjectResult ToProblem(IReadOnlyList<Error> errors)
        {
            var firstError = errors[0];
            var statusCode = firstError.ErrorType switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Failure => StatusCodes.Status400BadRequest,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                ErrorType.InvalidCredentials => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };
            var problem = new ProblemDetails
            {
                Title = firstError.Code,
                Detail = firstError.Description,
                Status = statusCode,
                Extensions = { ["errors"] = errors }
            };
            return new OkObjectResult(problem) { StatusCode = statusCode };
        }

        protected string GetEmailFromToken()
        {
            return User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException("No Email Claim Found!");
        }
    }
}
