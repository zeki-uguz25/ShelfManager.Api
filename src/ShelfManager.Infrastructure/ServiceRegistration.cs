using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ShelfManager.Application.Abstractions.Services;
using ShelfManager.Domain.Constants;
using ShelfManager.Infrastructure.Services;
using StackExchange.Redis;
using System.Text;

namespace ShelfManager.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IHashingService, HashingService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IBookCacheService, BookCacheService>();

        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!));

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "ShelfManager:";
        });

        services.AddAuthorization(options =>
        {
            var allPermissions = new[]
            {
                Permissions.Books.Read, Permissions.Books.Create, Permissions.Books.Update, Permissions.Books.Delete,
                Permissions.Categories.Manage,
                Permissions.Users.GetUser, Permissions.Users.Ban,
                Permissions.Roles.Manage,
                Permissions.UserBooks.Borrow, Permissions.UserBooks.Return,
                Permissions.Fines.Pay,
                Permissions.Notifications.Read
            };

            foreach (var permission in allPermissions)
            {
                options.AddPolicy(permission, policy =>
                    policy.RequireClaim("permission", permission));
            }
        });

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]!))
                };
            });

        return services;
    }
}
