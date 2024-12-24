using Arvant.Application.Common.Interfaces;
using Arvant.Application.Common.Models;
using Arvant.Application.Services;
using Arvant.Common.Dto.Call;
using Arvant.Domain.Entities;

namespace Arvant.Application.Calls.Commands;

public record CreateNewCallCommand(
    string Name,
    IEnumerable<Guid> ParticipantIds
): IRequest<Result<Guid>>;

public record Participant(Guid Id, string ConnectionId, string Name, string AvatarUrl);

public class CreateNewCallCommandHandler(IArvantContext arvantContext, ICurrentUser user,
    IArvantNotificationService arvantNotificationService) : IRequestHandler<CreateNewCallCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateNewCallCommand command, 
        CancellationToken cancellationToken) {
        var currentUserId = user.UserId;
        var call = Call.CreateNew(command.Name, currentUserId, command.ParticipantIds);
        await arvantContext.Calls.AddAsync(call, cancellationToken);
        await arvantContext.SaveChangesAsync(cancellationToken);
        var participants = await arvantContext.InternalUsers.Where(
                u => (command.ParticipantIds.Contains(u.Id) && u.Online && !string.IsNullOrEmpty(u.ConnectionId)) 
                     || u.Id == user.UserId)
            .Select(u => new Participant(u.Id, u.ConnectionId, u.Name, u.AvatarUrl))
            .ToListAsync(cancellationToken);
        var currentUser = participants.FirstOrDefault(p => p.Id == user.UserId);
        var participantsToCall = participants.Where(p => p.Id != user.UserId).ToList();
        foreach (var participant in participantsToCall) {
            await arvantNotificationService.SendIncomingCallEventAsync(new IncomingCallEventDto(
                call.Id, call.Name, 
                new CallerInfo(currentUser.Id, currentUser.Name, currentUser.AvatarUrl),
                null), 
                participant.ConnectionId
            );
        }
        return Result<Guid>.Success(call.Id);
    }
}
