using ProyectoPrueba.DTOs;
using System.Net;
using System.Text.Json;

namespace ProyectoPrueba.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Pasa la petición al siguiente componente (Controller, etc.)
                await _next(context);
            }
            catch (Exception ex)
            {
                // Si algo explota, lo capturamos aquí
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500

            // Si estamos en Desarrollo, mostramos el error real.
            // Si estamos en Producción, mostramos un mensaje genérico "Internal Server Error"
            var response = _env.IsDevelopment()
                ? new ErrorResponse
                {
                    StatusCode = context.Response.StatusCode,
                    Message = ex.Message,
                    Details = ex.StackTrace?.ToString()
                }
                : new ErrorResponse
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Error interno del servidor. Por favor intente más tarde.",
                    Details = "Internal Server Error"
                };

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}
