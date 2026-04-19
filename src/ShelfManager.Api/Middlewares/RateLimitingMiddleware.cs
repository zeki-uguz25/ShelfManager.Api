using Microsoft.AspNetCore.Http;
using StackExchange.Redis;
using System.Net;
using System.Text.Json;

namespace ShelfManager.Api.Middlewares
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConnectionMultiplexer _redis;

        private const int GlobalIpLimit = 100;
        private const int AuthIpLimit = 20;
        private const int AuthEmailLimit = 5;
        private const int WindowSeconds = 60;

        private static readonly HashSet<string> AuthEndpoints = new(StringComparer.OrdinalIgnoreCase)
        {
            "/api/auth/login",
            "/api/auth/register"
        };

        public RateLimitingMiddleware(RequestDelegate next, IConnectionMultiplexer redis)
        {
            _next = next;
            _redis = redis;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            /*
                Her HTTP isteği geldiğinde otomatik çalışan metottur.
                public async Task InvokeAsync(HttpContext context)
                HttpContext — o anki isteğin tüm bilgilerini taşır (IP, path, body, response vb.)
                Rate limit kontrollerini yapar, limit aşılmadıysa await _next(context) ile bir sonraki middleware'e isteği iletir.
            */
            var db = _redis.GetDatabase();
            var path = context.Request.Path.Value ?? string.Empty;
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var isAuthEndpoint = AuthEndpoints.Contains(path);

            // Global IP limiti (tüm API)
            var globalKey = $"ratelimit:global:ip:{ip}";
            var globalCount = await IncrementAsync(db, globalKey);
            if (globalCount > GlobalIpLimit)
            {
                await WriteRateLimitResponseAsync(context, "Çok fazla istek gönderdiniz. Lütfen 1 dakika sonra tekrar deneyin.");
                return;
            }

            if (isAuthEndpoint)
            {
                // Auth IP limiti
                var authIpKey = $"ratelimit:auth:ip:{ip}";
                var authIpCount = await IncrementAsync(db, authIpKey);
                if (authIpCount > AuthIpLimit)
                {
                    await WriteRateLimitResponseAsync(context, "Çok fazla deneme yaptınız. Lütfen 1 dakika sonra tekrar deneyin.");
                    return;
                }

                // Auth Email limiti
                var email = await GetEmailFromBodyAsync(context);
                if (!string.IsNullOrEmpty(email))
                {
                    var authEmailKey = $"ratelimit:auth:email:{email.ToLowerInvariant()}";
                    var authEmailCount = await IncrementAsync(db, authEmailKey);
                    if (authEmailCount > AuthEmailLimit)
                    {
                        await WriteRateLimitResponseAsync(context, "Bu hesap için çok fazla deneme yapıldı. Lütfen 1 dakika sonra tekrar deneyin.");
                        return;
                    }
                }
            }

            await _next(context);
        }

        private static async Task<long> IncrementAsync(IDatabase db, string key)
        {
            var count = await db.StringIncrementAsync(key);
            if (count == 1)
                await db.KeyExpireAsync(key, TimeSpan.FromSeconds(WindowSeconds));
            return count;
        }

        private static async Task<string?> GetEmailFromBodyAsync(HttpContext context)
        {
            try
            {
                context.Request.EnableBuffering();
                var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
                context.Request.Body.Position = 0;

                using var doc = JsonDocument.Parse(body);
                if (doc.RootElement.TryGetProperty("email", out var emailProp))
                    return emailProp.GetString();

                if (doc.RootElement.TryGetProperty("Email", out var emailPropAlt))
                    return emailPropAlt.GetString();
            }
            catch
            {
                // Body okunamazsa email bazlı limit atlanır
            }

            return null;
        }

        private static Task WriteRateLimitResponseAsync(HttpContext context, string message)
        {
            context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            context.Response.ContentType = "application/json";
            context.Response.Headers["Retry-After"] = WindowSeconds.ToString();

            var response = JsonSerializer.Serialize(new
            {
                StatusCode = (int)HttpStatusCode.TooManyRequests,
                Message = message,
                RetryAfter = WindowSeconds
            });

            return context.Response.WriteAsync(response);
        }
    }
}
