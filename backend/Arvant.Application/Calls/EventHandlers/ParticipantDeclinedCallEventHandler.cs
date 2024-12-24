using Arvant.Application.Common.Interfaces;
using Arvant.Domain.Enums;
using Arvant.Domain.Events;

namespace Arvant.Application.Calls.EventHandlers;

public class ParticipantDeclinedCallEventHandler(IArvantContext arvantContext)
    : INotificationHandler<ParticipantDeclinedCall>
{
    public async Task Handle(ParticipantDeclinedCall notification, CancellationToken cancellationToken) {
        var participantsCount = await arvantContext.CallParticipants.CountAsync(
            p => p.CallId == notification.CallId && p.ParticipantId != notification.ParticipantId, 
            cancellationToken: cancellationToken);
        if (participantsCount == 1) {
            var callInitiator = await arvantContext.CallParticipants.FirstOrDefaultAsync(
                p => p.CallId == notification.CallId && p.ParticipantId == p.Call.CreatedById, 
                cancellationToken: cancellationToken);
            if (callInitiator != null)
            {
                callInitiator.Status = CallParticipantStatus.Declined;
            }
            var call = await arvantContext.Calls.FirstOrDefaultAsync(c => c.Id == notification.CallId,
                cancellationToken: cancellationToken);
            call.Active = false;
        }
    }
}
