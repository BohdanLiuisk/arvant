using Arvant.Application.Common.Interfaces;
using Arvant.Application.Services;
using Arvant.WebApi.Services;
using Microsoft.OpenApi.Models;

namespace Arvant.WebApi.Extensions;

public static class ServicesExtensions
{
    public static void AddWebApiServices(this IServiceCollection services, IConfiguration configuration) {
        services.AddSwagger();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddControllers();
        services.AddCors(o => o.AddPolicy("CorsPolicy", b => {
            b.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins(configuration["ClientAppOrigin"]);
        }));
        services.AddSignalR();
        services.AddTransient<IArvantNotificationService, ArvantNotificationService>();
    }
    
    private static void AddSwagger(this IServiceCollection services) {
        services.AddSwaggerGen(options => {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                Description = "Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });
    }
}
