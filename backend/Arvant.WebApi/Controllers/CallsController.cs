using Arvant.Application.Calls.Commands;
using Arvant.Application.Calls.Queries;
using Arvant.Application.Common.Models;
using Arvant.Common.Dto;
using Arvant.Common.Dto.CallHistory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Arvant.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/calls")]
public class CallsController(IMediator mediator) : ControllerBase
{
    [HttpGet("getCalls")]
    public async Task<ActionResult<Result<PaginatedList<CallHistoryDto>>>> GetCalls(int pageNumber, int pageSize)
    {
        await Task.Delay(500);
        var users = await mediator.Send(
            new GetPaginatedCallsQuery(pageNumber, pageSize));
        return Ok(users);
    }
    
    [HttpGet("getById/{callId:guid}")]
    public async Task<ActionResult<Result<CurrentCallDto>>> GetById(Guid callId)
    {
        var users = await mediator.Send(new GetCurrentCallByIdQuery(callId));
        return Ok(users);
    }
    
    [HttpPost("createNew")]
    public async Task<ActionResult<Result<Guid>>> CreateNewCall(CreateNewCallCommand command)
    {
        var response = await mediator.Send(command);
        return Ok(response);
    }
    
    [HttpPost("joinCall")]
    public async Task<ActionResult<Result<bool>>> JoinCall(JoinCallCommand joinCallCommand)
    {
        var response = await mediator.Send(joinCallCommand);
        return Ok(response);
    }
    
    [HttpPost("leftCall")]
    public async Task<ActionResult<Result<bool>>> LeftCall(LeftCallCommand leftCallCommand)
    {
        var response = await mediator.Send(leftCallCommand);
        return Ok(response);
    }
    
    [HttpGet("searchParticipants")]
    public async Task<ActionResult<Result<ICollection<ParticipantForCallDto>>>> SearchParticipants(string searchValue)
    {
        var response = await mediator.Send(new SearchParticipantsForCallQuery(searchValue));
        return Ok(response);
    }
    
    [HttpGet("getCanJoin/{callId:guid}")]
    public async Task<ActionResult<Result<bool>>> GetCanJoin(Guid callId)
    {
        var response = await mediator.Send(new GetCanJoinCallQuery(callId));
        return Ok(response);
    }
}
