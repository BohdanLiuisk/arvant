using Arvant.Application.Common.Interfaces;
using Arvant.Domain.Enums;
using Arvant.Domain.Events;

namespace Arvant.Application.Calls.EventHandlers;

public class ParticipantLeftCallEventHandler(IArvantContext arvantContext) : INotificationHandler<ParticipantLeftCallEvent>
{
    public async Task Handle(ParticipantLeftCallEvent notification, CancellationToken cancellationToken) {
        var callId = notification.Participant.CallId;
        var call = await arvantContext.Calls
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(c => c.Id == callId, cancellationToken);
        var participantLeftCount = call.Participants.Count(
            p => p.Status is CallParticipantStatus.Active 
            or CallParticipantStatus.NotAnswered 
            or CallParticipantStatus.Declined);
        if (participantLeftCount == 0) {
            call.Active = false;
        }
    }
}
