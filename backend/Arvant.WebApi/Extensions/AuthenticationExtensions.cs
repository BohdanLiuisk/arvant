 using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Arvant.WebApi.Extensions;

public static class AuthenticationExtensions
{
    public static void AddArvantAuthentication(this IServiceCollection services) {
        services.AddAuthentication(options => {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => {
            options.TokenValidationParameters = new TokenValidationParameters {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                // ValidIssuer = "your_issuer",
                // ValidAudience = "your_audience",
                IssuerSigningKey = new SymmetricSecurityKey("6AD2EFDE-AB2C-4841-A05E-7045C855BA22"u8.ToArray()),
                ClockSkew = TimeSpan.Zero
            };
            options.Events = new JwtBearerEvents {
                OnMessageReceived = context => {
                    var accessToken = context.HttpContext.Request.Cookies["AccessToken"];
                    if (!string.IsNullOrEmpty(accessToken)) {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        });
    }
    
    private static Task HandleOnMessageReceived(MessageReceivedContext context) {
        var path = context.HttpContext.Request.Path;
        var accessToken = context.Request.Query["access_token"];
        var isHub = path.StartsWithSegments("/hubs/arvant") || path.StartsWithSegments("/hubs/call");
        if (!string.IsNullOrEmpty(accessToken) && isHub) {
            context.Token = accessToken;
        }
        return Task.CompletedTask;
    }
}
