using System.Security.Claims;
using Arvant.Application.Common.Interfaces;
using Arvant.Application.Common.Models;
using Arvant.Application.Users.Commands;
using Arvant.Application.Users.Queries;
using Arvant.Common.Dto;
using Arvant.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Arvant.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public class UsersController(IMediator mediator, IIdentityService identityService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("createNew")]
    public async Task<ActionResult<Result<Guid>>> CreateUser(CreateNewUserCommand newUser) {
        var userId = await mediator.Send(newUser);
        return Ok(userId);
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AppTokenInfo>> Login(LoginRequest loginRequest) {
        var loginResult = await identityService.LoginAsync(loginRequest.Email, loginRequest.Password);
        if (!loginResult.Succeeded) {
            return Unauthorized(loginResult);
        }
        return Ok(loginResult.Data);
    }
    
    [AllowAnonymous]
    [HttpPost("loginCookie")]
    public async Task<ActionResult<AppTokenInfo>> LoginCookie(LoginRequest loginRequest) {
        var loginResult = await identityService.LoginAsync(loginRequest.Email, loginRequest.Password);
        if (!loginResult.Succeeded) {
            return Unauthorized(loginResult);
        }
        SetAuthCookies(loginResult.Data);
        return Ok(Result.Success());
    }
    
    [AllowAnonymous]
    [HttpPost("refreshToken")]
    public async Task<ActionResult<AppTokenInfo>> RefreshToken(AppTokenInfo oldTokenInfo) {
        var refreshResult = await identityService.RefreshToken(oldTokenInfo);
        if (!refreshResult.Succeeded) {
            return Unauthorized(refreshResult);
        }
        return Ok(refreshResult.Data);
    }
    
    [AllowAnonymous]
    [HttpPost("refreshTokenCookie")]
    public async Task<ActionResult<AppTokenInfo>> RefreshTokenCookie() {
        if (!Request.Cookies.TryGetValue("AccessToken", out var accessToken) ||
            !Request.Cookies.TryGetValue("RefreshToken", out var refreshToken)) return Unauthorized();
        var refreshResult = await identityService.RefreshToken(new AppTokenInfo(accessToken, refreshToken));
        if (!refreshResult.Succeeded) {
            return Unauthorized(refreshResult);
        }
        SetAuthCookies(refreshResult.Data);
        return Ok();
    }
    
    [HttpGet("getById/{id:guid}")]
    public async Task<ActionResult<Result<UserDto>>> GetUser(Guid id)
    {
        var user = await mediator.Send(new GetUserByIdQuery(id));
        return Ok(user);
    }
    
    [HttpGet("getUsers")]
    public async Task<ActionResult<Result<PaginatedList<UserDto>>>> GetUsers(int pageNumber, int pageSize)
    {
        var users = await mediator.Send(
            new GetPaginatedUsersQuery(pageNumber, pageSize));
        return Ok(users);
    }
    
    [HttpGet("getCurrentUser")]
    public async Task<ActionResult<Result<UserDto>>> GetCurrentUser()
    {
        var currentUser = await mediator.Send(
            new GetCurrentUserQuery());
        return Ok(currentUser);
    }
    
    [HttpGet("getCurrentUser2")]
    public ActionResult<string> GetCurrentUser2() {
        var currentUser = HttpContext.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Name)
            ?.Value;
        return Ok(currentUser);
    }
    
    [HttpGet("getCurrentUser3")]
    public ActionResult<string> GetCurrentUser3() {
        var currentUser = HttpContext.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Name)
            ?.Value;
        return Ok($"{currentUser} - member only");
    }
    
    [Authorize(Roles = RoleNames.Admin)]
    [HttpGet("getCurrentUser4")]
    public ActionResult<string> GetCurrentUser4() {
        var currentUser = HttpContext.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Name)
            ?.Value;
        return Ok($"{currentUser} - admin only");
    }
    
    private void SetAuthCookies(AppTokenInfo tokenInfo) {
        Response.Cookies.Append("AccessToken", tokenInfo.AccessToken, new CookieOptions {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.Now.AddDays(30)
        });
        Response.Cookies.Append("RefreshToken", tokenInfo.RefreshToken, new CookieOptions {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.Now.AddDays(60)
        });
    }
}
