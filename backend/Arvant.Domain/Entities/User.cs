using Arvant.Domain.Common;

namespace Arvant.Domain.Entities;

public class User : BaseAuditableEntity<Guid>
{
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Name { get; set; }
    
    public string Login { get; set; }
    
    public string Email { get; set; }
    
    public string AvatarUrl { get; set; }
    
    public bool Online { get; set; }
    
    public string ConnectionId { get; set; }
}
