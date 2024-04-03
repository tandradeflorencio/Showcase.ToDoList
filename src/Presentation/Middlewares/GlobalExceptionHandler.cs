using Microsoft.AspNetCore.Diagnostics;
using Showcase.ToDoList.Domain.Models.Responses;

namespace Showcase.ToDoList.Presentation.Middlewares
{
    public class GlobalExceptionHandler(Serilog.ILogger logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.Error(exception, exception.Message);

            var baseResponse = new BaseResponse
            {
                Code = StatusCodes.Status500InternalServerError,
                Message = "Server error"
            };

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await httpContext.Response.WriteAsJsonAsync(baseResponse, cancellationToken);

            return true;
        }
    }
}