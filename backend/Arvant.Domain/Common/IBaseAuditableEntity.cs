using Arvant.Domain.Entities;

namespace Arvant.Domain.Common;

public interface IBaseAuditableEntity
{
    DateTime CreatedOn { get; set; }

    DateTime ModifiedOn { get; set; }

    Guid? CreatedById { get; set; }

    User CreatedBy { get; set; }

    Guid? ModifiedById { get; set; }

    User ModifiedBy { get; set; }
}
