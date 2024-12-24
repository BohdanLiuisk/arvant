using Arvant.Application.Common.Interfaces;
using Arvant.Application.Common.Models;
using Arvant.Common.Dto.Friends;
using Arvant.Application.Common;
using Arvant.Application.Common.Mapping;

namespace Arvant.Application.Friends.Queries;

public record FindPaginatedFriendsQuery(int PageNumber, int PageSize, string SearchValue) 
    : IRequest<Result<PaginatedList<FoundFriendDto>>>;

internal class FindPaginatedFriendsQueryHandler(IArvantContext arvantContext)
    : IRequestHandler<FindPaginatedFriendsQuery, Result<PaginatedList<FoundFriendDto>>>
{
    public async Task<Result<PaginatedList<FoundFriendDto>>> Handle(FindPaginatedFriendsQuery query, 
        CancellationToken cancellationToken) {
        var friends = await arvantContext.InternalUsers
            .OrderByDescending(u => u.CreatedOn)
            .Where(u => string.IsNullOrEmpty(query.SearchValue) || u.Name.Contains(query.SearchValue))
            .Select(u => new FoundFriendDto(u.Id, u.Name, u.AvatarUrl, new Random().Next(1, 11)))
            .PaginatedListAsync(query.PageNumber, query.PageSize);
        return Result<PaginatedList<FoundFriendDto>>.Success(friends);
    }
}
