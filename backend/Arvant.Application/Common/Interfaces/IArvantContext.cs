using Arvant.Domain.Entities;

namespace Arvant.Application.Common.Interfaces;

public interface IArvantContext
{
    DbSet<User> InternalUsers { get; }
    
    DbSet<Call> Calls { get; }
    
    DbSet<CallParticipant> CallParticipants { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
