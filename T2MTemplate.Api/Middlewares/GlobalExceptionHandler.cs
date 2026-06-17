using System.Text.Json;
using T2MTemplate.Domain.Exceptions;

namespace T2MTemplate.Api.Middlewares;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title) = MapException(exception);

        var response = new
        {
            titulo = title,
            status = statusCode,
            tipo = exception.GetType().Name,
            detalhes = exception.Message,
            instancia = $"{context.Request.Method} {context.Request.Path}"
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static (int StatusCode, string Title) MapException(Exception exception) => exception switch
    {
        NotFoundException => (StatusCodes.Status404NotFound, "Recurso não encontrado"),
        DomainException => (StatusCodes.Status400BadRequest, "Requisição inválida"),
        ArgumentException => (StatusCodes.Status400BadRequest, "Argumento inválido"),
        UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Acesso não autorizado"),
        _ => (StatusCodes.Status500InternalServerError, "Erro interno no servidor")
    };
}
