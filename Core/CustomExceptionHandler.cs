using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Transactions_Web_API.Core
{
  public class CustomExceptionHandler : IExceptionHandler
  {
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
        Exception exception, CancellationToken cancellationToken)
    {
      var statusCode = exception switch
      {
        BadHttpRequestException => StatusCodes.Status400BadRequest,
        UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
        _ => StatusCodes.Status500InternalServerError
      };

      var problemDetails = new ProblemDetails
      {
        Title = statusCode == StatusCodes.Status500InternalServerError
              ? "Internal Server Error"
              : "A handled exception occurred",
        Status = statusCode,
        Type = exception?.GetType().Name,
        Detail = exception?.Message,
        Instance = httpContext.Request.Path
      };

      await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

      return true;
    }
  }
}
