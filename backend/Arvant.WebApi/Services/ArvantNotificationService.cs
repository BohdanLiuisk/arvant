using Arvant.Application.Services;
using Arvant.Common.Dto.Call;
using Arvant.WebApi.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Arvant.WebApi.Services;

public class ArvantNotificationService : IArvantNotificationService
{
    private readonly IHubContext<ArvantHub> _hubContext;
    
    public ArvantNotificationService(IHubContext<ArvantHub> hubContext)
    {
        _hubContext = hubContext;
    }
    
    public async Task SendIncomingCallEventAsync(IncomingCallEventDto incomingCallEventDto, string connectionId)
    {
        await _hubContext.Clients.Client(connectionId).SendAsync("OnIncomingCall", incomingCallEventDto, connectionId);
    }
}
