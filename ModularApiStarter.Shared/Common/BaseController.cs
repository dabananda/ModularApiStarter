using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ModularApiStarter.Shared.Common
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public IActionResult Handle<T>(Result<T> result)
        {
            return result switch
            {
                { IsFailure: true, ExceptionType: ExceptionType.AlreadyExists } => StatusCode(StatusCodes.Status409Conflict, result),
                { IsFailure: true, ExceptionType: ExceptionType.Forbidden } => StatusCode(StatusCodes.Status403Forbidden, result),
                { IsFailure: true, ExceptionType: ExceptionType.NotFound } => StatusCode(StatusCodes.Status404NotFound, result),
                { IsFailure: true, ExceptionType: ExceptionType.Validation } => StatusCode(StatusCodes.Status400BadRequest, result),
                { IsFailure: true, ExceptionType: ExceptionType.Unauthorized } => StatusCode(StatusCodes.Status401Unauthorized, result),
                { IsFailure: true, ExceptionType: ExceptionType.InternalServerError } => StatusCode(StatusCodes.Status500InternalServerError, result),
                { IsFailure: true } => StatusCode(StatusCodes.Status400BadRequest, result),
                _ => StatusCode(StatusCodes.Status200OK, result)
            };
        }
    }
}
