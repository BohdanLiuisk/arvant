using Arvant.Domain.Common;

namespace Arvant.Domain.Events;

public class ParticipantDeclinedCall : BaseEvent
{
    public Guid CallId { get; set; }
    
    public Guid ParticipantId { get; set; }

    public ParticipantDeclinedCall(Guid callId, Guid participantId)
    {
        CallId = callId;
        ParticipantId = participantId;
    }
}
