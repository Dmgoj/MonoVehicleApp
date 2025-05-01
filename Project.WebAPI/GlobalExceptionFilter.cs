using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Project.Service.Common.Exceptions;

namespace Project.WebAPI
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            // 1) Grab the exception that was thrown
            var ex = context.Exception;

            // 2) Decide which HTTP status code to use based on exception type
            int statusCode = ex switch
            {
                ValidationException _ => StatusCodes.Status400BadRequest,
                NotFoundException _ => StatusCodes.Status404NotFound,
                ConflictException _ => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            // 3) Build a simple JSON error payload
            var errorResponse = new
            {
                error = ex.Message
            };

            // 4) Tell ASP.NET Core “here’s your response”
            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = statusCode
            };

            // 5) Mark the exception as handled so it doesn’t re-throw
            context.ExceptionHandled = true;
        }
    }
}
