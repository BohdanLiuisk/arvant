using Arvant.Application.Common.Interfaces;
using Arvant.Application.Common.Models;
using Arvant.Common.Dto;

namespace Arvant.Application.Users.Queries;

public record GetCurrentUserQuery: IRequest<Result<UserDto>>;

public class GetCurrentUserQueryHandler(IArvantContext arvantContext, ICurrentUser currentUser)
    : IRequestHandler<GetCurrentUserQuery, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken) {
        var user = await arvantContext.InternalUsers
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
            .FirstOrDefaultAsync(u => u.Id == currentUser.UserId, cancellationToken);
        user.IsCurrentUser = true;
        return Result<UserDto>.Success(user);
    }
}
