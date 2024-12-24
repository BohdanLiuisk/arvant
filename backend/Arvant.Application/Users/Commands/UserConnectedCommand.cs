using Arvant.Application.Common.Interfaces;
using Arvant.Application.Common.Models;

namespace Arvant.Application.Users.Commands;

public record UserConnectedCommand(
    string ConnectionId,
    Guid UserId
) : IRequest<Result>;

public class UserConnectedCommandHandler(IArvantContext arvantContext) : IRequestHandler<UserConnectedCommand, Result>
{
    public async Task<Result> Handle(UserConnectedCommand request, CancellationToken cancellationToken) {
        var user = await arvantContext.InternalUsers.FindAsync(request.UserId);
        user.ConnectionId = request.ConnectionId;
        user.Online = true;
        await arvantContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
