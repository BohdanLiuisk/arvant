using Arvant.Application.Calls.Commands;
using Arvant.Application.Common.Interfaces;
using Arvant.Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Arvant.WebApi.Hubs;

public class ArvantHub : Hub
{
    private readonly IMediator _mediator;

    private readonly ICurrentUser _currentUser;
    
    public ArvantHub(IMediator mediator, ICurrentUser currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        await _mediator.Send(new UserConnectedCommand(Context.ConnectionId, _currentUser.UserId));
    }
    
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await base.OnDisconnectedAsync(exception);
        await _mediator.Send(new UserDisconnectedCommand(Context.ConnectionId));
    }
    
    [HubMethodName("OnDeclineIncomingCall")]
    public async Task DeclineIncomingCall(DeclineIncomingCallCommand command)
    {
        await _mediator.Send(command);
    }
}
