using System.Security.Claims;
using Arvant.Application.Common.Interfaces;

namespace Arvant.WebApi.Services;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public Guid UserId {
        get {
            var idClaim = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(
                c => c.Type == ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(idClaim?.Value)) {
                return Guid.Parse(idClaim.Value);
            }
            throw new UnauthorizedAccessException("User context is not available.");
        }
    }

    public bool IsAvailable {
        get {
            var idClaim = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(
                c => c.Type == ClaimTypes.NameIdentifier);
            return idClaim?.Value != null && Guid.TryParse(idClaim.Value, out _);
        }
    }
}
