using Arvant.Domain.Common;
using Arvant.Domain.Enums;
using Arvant.Domain.Events;

namespace Arvant.Domain.Entities;

public class Call : BaseAuditableEntity<Guid>
{
    public string Name { get; set; }

    public Guid? DroppedById { get; set; }

    public User DroppedBy { get; set; }

    public bool Active { get; set; }

    public DateTime? EndDate { get; set; }

    public double Duration { get; set; }

    public ICollection<CallParticipant> Participants { get; private set; }

    private Call()
    {
        Participants = new List<CallParticipant>();
    }
    
    public static Call CreateNew(string name, Guid initiatorId, IEnumerable<Guid> participantIds)
    {
        var participants = new List<CallParticipant>();
        var initiatorParticipant = new CallParticipant
        {
            ParticipantId = initiatorId,
            Direction = CallDirection.Outgoing,
            Status = CallParticipantStatus.NotActive
        };
        participants.Add(initiatorParticipant);
        participants.AddRange(participantIds.Select(participantId => new CallParticipant
        {
            ParticipantId = participantId, 
            Direction = CallDirection.Incoming, 
            Status = CallParticipantStatus.NotAnswered
        }));
        var call = new Call
        {
            Name = name,
            Active = true,
            Participants = participants
        };
        call.AddDomainEvent(new NewCallCreatedEvent(call));
        return call;
    }

    public bool GetCanJoin(Guid participantId)
    {
        return Active || Participants.Any(p => p.ParticipantId == participantId && p.Active);
    }
}
