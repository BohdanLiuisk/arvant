using Arvant.Application.Common.Interfaces;
using Arvant.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Arvant.Infrastructure.Context.Interceptors;

public class AuditableEntityInterceptor(ICurrentUser currentUser) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result) {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, 
        InterceptionResult<int> result, CancellationToken cancellationToken = default) {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context) {
        if(context == null) return;
        foreach (var entry in context.ChangeTracker.Entries().Where(e => e.Entity is IBaseAuditableEntity)) {
            var auditableEntity = (IBaseAuditableEntity)entry.Entity;
            if (entry.State == EntityState.Added) {
                if (currentUser.IsAvailable)
                {
                    auditableEntity.CreatedById = currentUser.UserId;
                }
                auditableEntity.CreatedOn = DateTime.UtcNow;
            }
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified 
                                                 || entry.HasChangedOwnedEntities()) {
                if (currentUser.IsAvailable) {
                    auditableEntity.ModifiedById = currentUser.UserId;
                }
                auditableEntity.ModifiedOn = DateTime.UtcNow;
            }
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r => 
            r.TargetEntry != null && 
            r.TargetEntry.Metadata.IsOwned() && 
            r.TargetEntry.State is EntityState.Added or EntityState.Modified);
}
