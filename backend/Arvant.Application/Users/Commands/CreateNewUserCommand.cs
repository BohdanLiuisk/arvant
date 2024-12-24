using Arvant.Application.Common.Interfaces;
using Arvant.Application.Common.Models;
using Arvant.Domain.Entities;

namespace Arvant.Application.Users.Commands;

public record CreateNewUserCommand(
    string FirstName,
    string LastName,
    string Login, 
    string Password, 
    string Email
) : IRequest<Result<Guid>>;

public class CreateNewUserCommandHandler(IArvantContext arvantContext, IIdentityService identityService)
    : IRequestHandler<CreateNewUserCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateNewUserCommand command, 
        CancellationToken cancellationToken) {
        var result = await identityService.CreateUserAsync(command.Login, command.Password);
        if (result.IsFailure) {
            return result;
        }
        var user = new User {
            Id = result.Data,
            FirstName = command.FirstName,
            LastName = command.LastName,
            Name = $"{command.FirstName} {command.LastName}",
            Login = command.Login,
            Email = command.Email
        };
        await arvantContext.InternalUsers.AddAsync(user, cancellationToken);
        await arvantContext.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Success(user.Id);
    }
}
