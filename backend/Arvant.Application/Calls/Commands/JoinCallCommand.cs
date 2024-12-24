using Arvant.Application.Common.Interfaces;
using Arvant.Application.Common.Models;
using Arvant.Domain.Enums;

namespace Arvant.Application.Calls.Commands;

public record JoinCallCommand : IRequest<Result>
{
    public Guid CallId { get; set; }
    public string StreamId { get; set; }
    public string PeerId { get; set; }
    public string ConnectionId { get; set; }
}

public class JoinCallCommandHandler(IArvantContext arvantContext, ICurrentUser currentUser)
    : IRequestHandler<JoinCallCommand, Result>
{
    public async Task<Result> Handle(JoinCallCommand command, CancellationToken cancellationToken) {
        var call = await arvantContext.Calls
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(c => c.Id == command.CallId, cancellationToken);
        if (call is null) {
            return Result.Failure(["You cannot join this call as it does not exist."]);
        }
        if (!call.Active) {
            return Result.Failure(["You cannot join this call as it has ended."]);
        }
        var participant = call.Participants
            .FirstOrDefault(p => p.ParticipantId == currentUser.UserId);
        if (participant is not null) {
            if (participant.Status == CallParticipantStatus.Active) {
                return Result.Failure(["You are already in a call."]);
            }
            participant.Status = CallParticipantStatus.Active;
            participant.StreamId = command.StreamId;
            participant.PeerId = command.PeerId;
            participant.ConnectionId = command.ConnectionId;
        } else {
            return Result.Failure(["You are not allowed to join this call."]);
        }
        await arvantContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
