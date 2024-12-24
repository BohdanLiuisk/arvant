using Arvant.Entity.Extensions;
using Arvant.Application.Common.Interfaces;
using Arvant.Entity.Context;
using Arvant.Infrastructure.Context;
using Arvant.Infrastructure.Context.Interceptors;
using Arvant.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arvant.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration) {
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        var dbConnectionString = configuration.GetConnectionString("ArvantDb");
        services.AddDbContext<AppDbContext>((sp, options) => {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(dbConnectionString, builder => 
                builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)).UseSnakeCaseNamingConvention();
        });
        services.AddArvantEntity(options => {
            options.ConnectionString = dbConnectionString;
            options.ConfigureDbContext = contextBuilder => {
                contextBuilder.UseNpgsql(dbConnectionString, builder => builder
                        .MigrationsAssembly(typeof(ArvantEntityContext).Assembly.FullName))
                        .UseSnakeCaseNamingConvention();
            };
        });
        services.AddScoped<IArvantContext>(provider => provider.GetRequiredService<AppDbContext>());
        services.AddScoped<ArvantContextInitializer>();
        services.AddDefaultIdentity<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();
        services.AddTransient<IIdentityService, IdentityService>();
    }
}
