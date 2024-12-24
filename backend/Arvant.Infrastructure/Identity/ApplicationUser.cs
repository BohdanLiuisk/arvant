using Microsoft.AspNetCore.Identity;

namespace Arvant.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    
    public DateTime RefreshTokenExpiry { get; set; }
}
