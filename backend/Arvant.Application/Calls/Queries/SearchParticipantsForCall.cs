using Arvant.Application.Common.Interfaces;
using Arvant.Application.Common.Models;
using Arvant.Common.Dto;

namespace Arvant.Application.Calls.Queries;

public record SearchParticipantsForCallQuery(
    string SearchValue
): IRequest<Result<ICollection<ParticipantForCallDto>>>;

public class SearchParticipantsForCallQueryHandler(IArvantContext arvantContext)
    : IRequestHandler<SearchParticipantsForCallQuery,
        Result<ICollection<ParticipantForCallDto>>>
{
    public async Task<Result<ICollection<ParticipantForCallDto>>> Handle(SearchParticipantsForCallQuery query, 
        CancellationToken cancellationToken) {
        var participants = await arvantContext.InternalUsers
            .Take(50)
            .Where(u => string.IsNullOrEmpty(query.SearchValue) || u.Name.Contains(query.SearchValue))
            .Select(u => new ParticipantForCallDto {
                Id = u.Id,
                Username = u.Login,
                Name = u.Name,
                IsOnline = u.Online,
                AvatarUrl = u.AvatarUrl
            })
            .ToListAsync(cancellationToken);
        return Result<ICollection<ParticipantForCallDto>>.Success(participants);
    }
}
