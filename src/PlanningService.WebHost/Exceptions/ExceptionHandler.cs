using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PlanningService.Application.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace PlanningService.WebHost.Exceptions;

public class ExceptionHandler : IExceptionHandler
{
    private readonly IHostEnvironment _environment;
    private readonly ILogger<ExceptionHandler> _logger;

    public ExceptionHandler(IHostEnvironment environment, ILogger<ExceptionHandler> logger)
    {
        _environment = environment;
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Handling exception {ExceptionName}", exception.GetType().Name);

        switch (exception)
        {
            case ArgumentException:
            case ValidationException:
                await HandleExceptionAsync(httpContext, exception, StatusCodes.Status400BadRequest, cancellationToken);
            break;

            case NotFoundException:
                await HandleExceptionAsync(httpContext, exception, StatusCodes.Status404NotFound, cancellationToken);
            break;

            case NotImplementedException:
                await HandleExceptionAsync(httpContext, exception, StatusCodes.Status501NotImplemented, cancellationToken);
            break;

            default:
                await HandleExceptionAsync(httpContext, exception, StatusCodes.Status500InternalServerError, cancellationToken);
            break;
        }

        return true;
    }

    private async Task<bool> HandleExceptionAsync(HttpContext httpContext, Exception exception, int status, CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Title = exception.Message,
            Status = status 
        };

        if (_environment.IsDevelopment())
        {
            problemDetails.Extensions["trace"] = exception.StackTrace;
        }

        httpContext.Response.StatusCode = status;
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
