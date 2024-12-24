using Arvant.Application.Common.Interfaces;
using Arvant.Application.Common.Models;
using Arvant.Common.Dto;

namespace Arvant.Application.Users.Queries;

public record GetUserByIdQuery(Guid Id) : IRequest<Result<UserDto>>;

public class GetUserByIdQueryHandler(IArvantContext arvantContext, ICurrentUser currentUser)
    : IRequestHandler<GetUserByIdQuery, Result<UserDto>> 
{
    public async Task<Result<UserDto>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken) {
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
            .FirstOrDefaultAsync(u => u.Id == query.Id, cancellationToken);
        if (user == null) {
            return Result<UserDto>.Failure([$"User with id {query.Id} was not found."]);
        }
        user.IsCurrentUser = user.Id == currentUser.UserId;
        return Result<UserDto>.Success(user);
    }
}
