using Arvant.Entity.Descriptor;
using Arvant.Entity.Extensions;
using Arvant.Entity.Structure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Arvant.Entity.Initialization;

internal class ArvantEntityHostedService(ArvantEntityOptions arvantEntityOptions, IServiceProvider serviceProvider) 
    : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken) {
        IList<TableDescriptor>? tableDescriptors = null;
        if (arvantEntityOptions.LoadTablesFromOuterStore != null) {
            tableDescriptors = (await arvantEntityOptions.LoadTablesFromOuterStore()).ToList();
        }
        var initConfig = new EntityStructureInitConfig {
            OuterTables = tableDescriptors
        };
        using var scope = serviceProvider.CreateScope();
        var entityStructureManager = scope.ServiceProvider.GetRequiredService<EntityStructureManager>();
        await entityStructureManager.InitializeAsync(initConfig);
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        return Task.CompletedTask;
    }
}
