using Arvant.Application.Common.Interfaces;
using Arvant.Application.Common.Models;

namespace Arvant.Application.Calls.Commands;

public record LeftCallCommand(Guid CallId): IRequest<Result>;

public class LeftCallCommandHandler(IArvantContext arvantContext, ICurrentUser currentUser)
    : IRequestHandler<LeftCallCommand, Result>
{
    public async Task<Result> Handle(LeftCallCommand command, CancellationToken cancellationToken) {
        var call = await arvantContext.Calls
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(c => c.Id == command.CallId, cancellationToken);
        if (call is null) {
            return Result.Failure(["You cannot leave this call as it does not exist."]);
        }
        if (!call.Active) {
            return Result.Failure(["Call is already ended."]);
        }
        var participant = call.Participants.FirstOrDefault(
            p => p.ParticipantId == currentUser.UserId);
        if (participant is null) {
            return Result.Failure(["You cannot leave this call as you are not in it."]);
        }
        participant.LeftCall();
        await arvantContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
