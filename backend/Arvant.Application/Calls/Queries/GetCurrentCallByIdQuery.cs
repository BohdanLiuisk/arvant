using Arvant.Application.Common.Interfaces;
using Arvant.Application.Common.Models;
using Arvant.Common.Dto;

namespace Arvant.Application.Calls.Queries;

public record GetCurrentCallByIdQuery(Guid CallId): IRequest<Result<CurrentCallDto>>;

public class GetCurrentCallByIdQueryHandler(IArvantContext arvantContext, ICurrentUser currentUser)
    : IRequestHandler<GetCurrentCallByIdQuery, Result<CurrentCallDto>>
{
    public async Task<Result<CurrentCallDto>> Handle(GetCurrentCallByIdQuery query, 
        CancellationToken cancellationToken) {
        var callWithParticipants = await arvantContext.Calls
            .Include(c => c.Participants)
            .ThenInclude(p => p.Participant)
            .Select(c => new {
                Call = new CallDto {
                    Id = c.Id,
                    Name = c.Name,
                    Active = c.Active,
                    InitiatorId = c.CreatedById,
                    StartDate = c.CreatedOn,
                    EndDate = c.EndDate,
                    Duration = c.Duration,
                    Direction = (int)c.Participants
                        .FirstOrDefault(p => p.ParticipantId == currentUser.UserId).Direction,
                },
                Participants = c.Participants.Select(p => new CallParticipantDto {
                    Id = p.Id,
                    ParticipantId = p.ParticipantId,
                    Name = p.Participant.Name,
                    StreamId = p.StreamId,
                    PeerId = p.PeerId,
                    ConnectionId = p.ConnectionId,
                    Active = p.Active,
                    JoinedAt = p.JoinedAt
                }).ToList()
            })
            .FirstOrDefaultAsync(c => c.Call.Id == query.CallId, cancellationToken);
        if (callWithParticipants is null) {
            return Result<CurrentCallDto>.Failure([$"Call with id {query.CallId} not found."]);
        }
        var currentCallDto = new CurrentCallDto(callWithParticipants.Call, callWithParticipants.Participants);
        return Result<CurrentCallDto>.Success(currentCallDto);
    }
}
