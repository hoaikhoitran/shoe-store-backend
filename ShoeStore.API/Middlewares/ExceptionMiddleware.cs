using System.Net;
using System.Text.Json;
using ShoeStore.API.Core.Exceptions;

namespace ShoeStore.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
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
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                await WriteResponse(context, ex.Message, HttpStatusCode.NotFound);
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex.Message);
                await WriteResponse(context, ex.Message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await WriteResponse(context, "Internal server error", HttpStatusCode.InternalServerError);
            }
        }

        private static async Task WriteResponse(
            HttpContext context,
            string message,
            HttpStatusCode statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                statusCode = context.Response.StatusCode,
                message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
