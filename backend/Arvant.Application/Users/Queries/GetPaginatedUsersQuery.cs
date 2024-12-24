using Arvant.Application.Common.Interfaces;
using Arvant.Application.Common.Models;
using Arvant.Common.Dto;
using Arvant.Application.Common.Mapping;

namespace Arvant.Application.Users.Queries;

public record GetPaginatedUsersQuery(
    int PageNumber, int PageSize
) : IRequest<Result<PaginatedList<UserDto>>>;

public class GetPaginatedUsersQueryHandler(IArvantContext arvantContext) 
    : IRequestHandler<GetPaginatedUsersQuery, Result<PaginatedList<UserDto>>>
{
    public async Task<Result<PaginatedList<UserDto>>> Handle(GetPaginatedUsersQuery query, 
        CancellationToken cancellationToken) {
        var users = await arvantContext.InternalUsers
            .OrderBy(u => u.Name)
            .Select(u => new UserDto {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Name = u.Name,
                Login = u.Login,
                Email = u.Email,
                AvatarUrl = u.AvatarUrl,
                Online = u.Online,
                CreatedOn = u.CreatedOn
            })
            .PaginatedListAsync(query.PageNumber, query.PageSize);
        return Result<PaginatedList<UserDto>>.Success(users);
    }
}
