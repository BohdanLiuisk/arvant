using Arvant.Domain.Common;
using Arvant.Domain.Entities;

namespace Arvant.Domain.Events;

public class ParticipantLeftCallEvent : BaseEvent
{
    public CallParticipant Participant { get; set; }
    
    public ParticipantLeftCallEvent(CallParticipant participant)
    {
        Participant = participant;
    }
}
