using Arvant.Entity.Abstract;
using Arvant.Entity.Context;
using Arvant.Entity.DbEngine;
using Arvant.Entity.Initialization;
using Arvant.Entity.SelectQuery;
using Arvant.Entity.Structure;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Arvant.Entity.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddArvantEntity(this IServiceCollection services, 
        Action<ArvantEntityOptions>? arvantEntityOptionsAction = null) {
        var arvantEntityOptions = new ArvantEntityOptions();
        arvantEntityOptionsAction?.Invoke(arvantEntityOptions);
        services.AddSingleton(arvantEntityOptions);
        services.AddDbContext<ArvantEntityContext>(arvantEntityOptions.ConfigureDbContext);
        services.AddFluentMigratorCore().ConfigureRunner(rb => rb.AddPostgres()
            .WithGlobalConnectionString(arvantEntityOptions.ConnectionString));
        services.AddScoped<EntityStructureManager>();
        services.AddTransient<IEntityDbEngine, EntityDbEngine>();
        services.AddTransient<ISelectQueryExecutor, SelectQueryExecutor>();
        services.AddTransient<AliasStorage>();
        services.AddTransient<QueryFactory>(_ => {  
            var connection = new NpgsqlConnection(arvantEntityOptions.ConnectionString);  
            var compiler = new PostgresCompiler();  
            return new QueryFactory(connection, compiler);  
        });
        services.AddHostedService<ArvantEntityHostedService>();
    }
}
