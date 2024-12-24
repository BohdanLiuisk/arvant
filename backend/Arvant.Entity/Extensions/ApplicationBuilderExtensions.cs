using Arvant.Entity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Arvant.Entity.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void UseArvantEntity(this IHost app) {
        using var scope = app.Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<ArvantEntityContext>().Database.Migrate();
    }
}
