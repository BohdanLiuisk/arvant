using Arvant.Application.Common.Interfaces;
using Arvant.Application.Common.Models;
using Arvant.Common.Dto.CallHistory;
using Arvant.Application.Common.Mapping;

namespace Arvant.Application.Calls.Queries;

public record GetPaginatedCallsQuery(int PageNumber, int PageSize) : IRequest<Result<PaginatedList<CallHistoryDto>>>;

public class GetPaginatedCallsQueryHandler(IArvantContext arvantContext, ICurrentUser currentUser)
    : IRequestHandler<GetPaginatedCallsQuery, Result<PaginatedList<CallHistoryDto>>>
{
    public async Task<Result<PaginatedList<CallHistoryDto>>> Handle(GetPaginatedCallsQuery query, 
        CancellationToken cancellationToken)
    {
        var currentUserId = currentUser.UserId;
        var calls = await arvantContext.Calls
            .OrderByDescending(u => u.CreatedOn)
            .Where(c => c.Participants.Any(p => p.ParticipantId == currentUserId))
            .Select(c => new CallHistoryDto {
                Id = c.Id,
                Name = c.Name,
                Active = c.Active,
                InitiatorId = c.CreatedById,
                StartDate = c.CreatedOn,
                Direction = (int)c.Participants
                    .FirstOrDefault(p => p.ParticipantId == currentUserId).Direction,
                Status = (int)c.Participants
                    .FirstOrDefault(p => p.ParticipantId == currentUserId).Status,
                Participants = c.Participants
                    .Where(p => p.ParticipantId != currentUserId)
                    .Select(p => new CallParticipantHistoryDto {
                        Id = p.Participant.Id,
                        CallParticipantId = p.Id,
                        Name = p.Participant.Name,
                        AvatarUrl = p.Participant.AvatarUrl,
                        IsOnline = p.Participant.Online,
                        CallDirection = (int)p.Direction,
                        Status = (int)p.Status
                    })
            })
            .PaginatedListAsync(query.PageNumber, query.PageSize);
        return Result<PaginatedList<CallHistoryDto>>.Success(calls);
    }
}
