using Arvant.Application.Common.Models;
using Arvant.Application.Friends.Queries;
using Arvant.Common.Dto.Friends;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Arvant.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/friends")]
public class FriendsController(IMediator mediator) : ControllerBase
{
    [HttpGet("findFriends")]
    public async Task<ActionResult<Result<PaginatedList<FoundFriendDto>>>>
        FindFriends(int pageNumber, int pageSize, string searchValue) {
        var friends = await mediator.Send(new FindPaginatedFriendsQuery(pageNumber, pageSize, searchValue));
        return Ok(friends);
    }
}
