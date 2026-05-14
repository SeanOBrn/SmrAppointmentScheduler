using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SmrAppointmentScheduler.Server.Common.Exceptions;

namespace SmrAppointmentScheduler.Server.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (System.Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, System.Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = new { error = exception.Message };

        switch (exception)
        {
            case NotFoundException:
                code = HttpStatusCode.NotFound;
                break;
            case ConflictException:
                code = HttpStatusCode.Conflict;
                break;
            case BadRequestException:
                code = HttpStatusCode.BadRequest;
                break;
        }

        var payload = JsonSerializer.Serialize(result);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(payload);
    }
}
