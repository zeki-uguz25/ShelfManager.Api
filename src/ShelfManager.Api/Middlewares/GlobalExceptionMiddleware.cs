using Core.Exception.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace ShelfManager.Api.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
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
            var statusCode = exception switch
            {
                NotFoundException     => HttpStatusCode.NotFound,
                BusinessException     => HttpStatusCode.BadRequest,
                UnauthorizedException => HttpStatusCode.Unauthorized,
                _                     => HttpStatusCode.InternalServerError
            };

            var message = exception is NotFoundException or BusinessException or UnauthorizedException
                ? exception.Message
                : "Beklenmeyen bir hata oluştu.";

            var response = JsonSerializer.Serialize(new
            {
                StatusCode = (int)statusCode,
                Message = message
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(response);
        }
    }
}
