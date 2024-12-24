using Arvant.Domain.Common;
using Arvant.Domain.Enums;
using Arvant.Domain.Events;

namespace Arvant.Domain.Entities;

public class CallParticipant : BaseEntity<int>
{
    public Guid CallId { get; set; }
    
    public Call Call { get; set; }

    public Guid ParticipantId { get; set; }
    
    public User Participant { get; set; }
    
    public DateTime JoinedAt { get; set; }
    
    public bool Active { get; set; }
    
    public string StreamId { get; set; }
    
    public string PeerId { get; set;  }
    
    public string ConnectionId { get; set; }
    
    public CallDirection Direction { get; set; }
    
    public CallParticipantStatus Status { get; set; }

    public void DeclineCall()
    {
        Status = CallParticipantStatus.Declined;
        Active = false;
        AddDomainEvent(new ParticipantDeclinedCall(CallId, ParticipantId));
    }

    public void LeftCall()
    {
        Active = false;
        StreamId = string.Empty;
        PeerId = string.Empty;
        ConnectionId = string.Empty;
        Status = CallParticipantStatus.NotActive;
        AddDomainEvent(new ParticipantLeftCallEvent(this));
    }
}
