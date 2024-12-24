using Arvant.Application.Common.Interfaces;
using Arvant.Application.Common.Models;
using Arvant.Common.Dto.Call;

namespace Arvant.Application.Calls.Queries;

public record GetCanJoinCallQuery(Guid CallId): IRequest<Result<CanJoinCallDto>>;

public class GetCanJoinCallQueryHandler(IArvantContext arvantContext, ICurrentUser currentUser)
    : IRequestHandler<GetCanJoinCallQuery, Result<CanJoinCallDto>>
{
    public async Task<Result<CanJoinCallDto>> Handle(GetCanJoinCallQuery query, CancellationToken cancellationToken) {
        var call = await arvantContext.Calls
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(c => c.Id == query.CallId, cancellationToken);
        var success = call.GetCanJoin(currentUser.UserId);
        var errorMessage = !success ? "Call is already over." : string.Empty;
        return Result<CanJoinCallDto>.Success(new CanJoinCallDto(success, errorMessage));
    }
}
