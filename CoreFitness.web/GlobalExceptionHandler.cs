using CoreFitness.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace CoreFitness.Web;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        var (statusCode, logAsError) = exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound,  false),
            ValidationException => (StatusCodes.Status400BadRequest, false),
            ConflictException => (StatusCodes.Status409Conflict, false),
            BusinessRuleException => (StatusCodes.Status422UnprocessableEntity, false),
            Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException => (StatusCodes.Status409Conflict, false),
            _ => (StatusCodes.Status500InternalServerError, true)
        };

        if (logAsError)
                logger.LogError(exception, "Exception occorred: {Message}", exception.Message);
            else
                logger.LogWarning(exception, "Request failed: {Message}", exception.Message);

        context.Response.Redirect($"/error?statusCode={statusCode}");            

        return ValueTask.FromResult(true);
    }

}
