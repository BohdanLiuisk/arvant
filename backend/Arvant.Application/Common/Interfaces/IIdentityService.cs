using Arvant.Application.Common.Models;

namespace Arvant.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<Result<Guid>> CreateUserAsync(string userName, string password);
    
    Task<Result<AppTokenInfo>> LoginAsync(string userId, string policyName);

    Task<Result<AppTokenInfo>> RefreshToken(AppTokenInfo oldTokenInfo);
    
    Task<Result> DeleteUserAsync(string userId);
}
