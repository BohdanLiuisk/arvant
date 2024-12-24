using Arvant.Application.Common.Interfaces;
using Arvant.Application.Common.Models;

namespace Arvant.Application.Users.Commands;

public record UserDisconnectedCommand(
    string ConnectionId
) : IRequest<Result>;

public class UserDisconnectedCommandHandler(IArvantContext arvantContext) : IRequestHandler<UserDisconnectedCommand, Result>
{
    public async Task<Result> Handle(UserDisconnectedCommand command, CancellationToken cancellationToken) {
        var user = await arvantContext.InternalUsers.FirstOrDefaultAsync(
            u => u.ConnectionId == command.ConnectionId, cancellationToken);
        if (user is not null) {
            user.ConnectionId = null;
            user.Online = false;
            await arvantContext.SaveChangesAsync(cancellationToken);
        }
        return Result.Success();
    }
}
