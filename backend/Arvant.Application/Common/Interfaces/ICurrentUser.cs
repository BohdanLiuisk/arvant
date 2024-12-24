namespace Arvant.Application.Common.Interfaces;

public interface ICurrentUser
{
    Guid UserId { get; }
    
    bool IsAvailable { get; }
}
