using Backend.Utils.CustomExceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Utils.ExceptionHandlers
{
    internal sealed class EmailAlreadyInUseExceptionHandler : IExceptionHandler
    {

        private readonly ILogger<EmailAlreadyInUseExceptionHandler> logger;

        public EmailAlreadyInUseExceptionHandler(ILogger<EmailAlreadyInUseExceptionHandler> logger)
        {
            this.logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not EmailAlreadyInUseException emailAlreadyInUseException)
            {
                return false;
            }
            logger.LogError(emailAlreadyInUseException, emailAlreadyInUseException.Message);


            var problemDetails = new ProblemDetails
            {
                Status = 409,
                Title = "Conflict",
                Detail = emailAlreadyInUseException.Message
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    
    }
}
