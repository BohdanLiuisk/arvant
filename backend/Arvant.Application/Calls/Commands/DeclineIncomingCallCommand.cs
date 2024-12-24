using Arvant.Application.Common.Interfaces;
using Arvant.Application.Common.Models;

namespace Arvant.Application.Calls.Commands;

public record DeclineIncomingCallCommand(Guid CallId) : IRequest<Result>;

public class DeclineIncomingCallCommandHandler(IArvantContext arvantContext, ICurrentUser currentUser)
    : IRequestHandler<DeclineIncomingCallCommand, Result>
{
    public async Task<Result> Handle(DeclineIncomingCallCommand command, CancellationToken cancellationToken) {
        var callParticipant = await arvantContext.CallParticipants
            .FirstOrDefaultAsync(c => c.CallId == command.CallId && c.ParticipantId == currentUser.UserId, 
                cancellationToken);
        callParticipant.DeclineCall();
        await arvantContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
