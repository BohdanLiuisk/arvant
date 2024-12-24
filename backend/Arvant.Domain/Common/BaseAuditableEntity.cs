using Arvant.Domain.Entities;

namespace Arvant.Domain.Common;

public abstract class BaseAuditableEntity<T> : BaseEntity<T>, IBaseAuditableEntity where T: struct
{
    public DateTime CreatedOn { get; set; }

    public DateTime ModifiedOn { get; set; }

    public Guid? CreatedById { get; set; }

    public User CreatedBy { get; set; }

    public Guid? ModifiedById { get; set; }

    public User ModifiedBy { get; set; }
}
