using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.API.ExceptionHandler
{
    public class GlobalExceptionHandler(IProblemDetailsService problemDetailsService)
        : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
            Exception exception, 
            CancellationToken cancellationToken)
        {
            ProblemDetails problemDetails;

            if (exception is ApiException apiException)
            {
                problemDetails = new ProblemDetails
                {
                    Status = apiException.Code,
                    Title = apiException.Title,
                    Type = apiException.GetType().Name,
                    Detail = apiException.Details
                };
            }
            else
            {
                problemDetails = new ProblemDetails
                {
                    Status = 500,
                    Title = "Undefined Interal Server Error",
                    Type = exception.GetType().Name,
                    Detail = exception.Message
                };
            }

            httpContext.Response.StatusCode = problemDetails.Status ?? 500;

            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                Exception = exception,
                HttpContext = httpContext,
                ProblemDetails = problemDetails
            });
        }
    }
}
