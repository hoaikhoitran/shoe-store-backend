using System.Net;
using System.Text.Json;
using ShoeStore.API.Core;

namespace ShoeStore.API.Middlewares
{
    // Middleware này dùng để BẮT TOÀN BỘ exception trong pipeline HTTP
    // => Tránh try-catch lặp lại ở Controller / Service
    // => Trả về JSON + HTTP status code chuẩn cho client
    public class ExceptionMiddleware
    {
        // Delegate đại diện cho middleware tiếp theo trong pipeline
        private readonly RequestDelegate _next;

        // Logger dùng để ghi log lỗi (warning / error)
        private readonly ILogger<ExceptionMiddleware> _logger;

        // Constructor được DI container inject tự động
        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;       // middleware kế tiếp
            _logger = logger;   // logger cho middleware này
        }

        // Hàm chính của middleware
        // MỖI request HTTP đều đi qua hàm này
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Cho request chạy tiếp xuống các middleware sau
                // (Routing, Auth, Controller, Service, ...)
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                // Bắt lỗi nghiệp vụ: không tìm thấy dữ liệu
                // Log mức cảnh báo
                _logger.LogWarning(ex.Message);

                // Trả về HTTP 404 + JSON message
                await WriteResponse(context, ex.Message, HttpStatusCode.NotFound);
            }
            catch (BadRequestException ex)
            {
                // Bắt lỗi nghiệp vụ: request sai (validation, rule business)
                _logger.LogWarning(ex.Message);

                // Trả về HTTP 400
                await WriteResponse(context, ex.Message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                // Bắt TẤT CẢ lỗi còn lại (lỗi hệ thống, null, db, ...)
                _logger.LogError(ex, ex.Message);

                // Không trả message chi tiết để tránh lộ thông tin nội bộ
                await WriteResponse(
                    context,
                    "Internal server error",
                    HttpStatusCode.InternalServerError
                );
            }
        }

        // Hàm dùng chung để ghi response JSON cho client
        private static async Task WriteResponse(
            HttpContext context,
            string message,
            HttpStatusCode statusCode)
        {
            // Response trả về là JSON
            context.Response.ContentType = "application/json";

            // Set HTTP status code (400, 404, 500, ...)
            context.Response.StatusCode = (int)statusCode;

            // Object response chuẩn cho frontend
            var response = new
            {
                statusCode = context.Response.StatusCode,
                message
            };

            // Serialize object sang JSON và ghi ra response
            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response)
            );
        }
    }
}
