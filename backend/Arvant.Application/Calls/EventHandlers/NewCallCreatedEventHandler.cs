using Arvant.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Arvant.Application.Calls.EventHandlers;

public class NewCallCreatedEventHandler(ILogger<NewCallCreatedEventHandler> logger)
    : INotificationHandler<NewCallCreatedEvent>
{
    public Task Handle(NewCallCreatedEvent notification, CancellationToken cancellationToken) {
        logger.LogInformation($"New call created: {notification.Call.Name}");
        return Task.CompletedTask;
    }
}
