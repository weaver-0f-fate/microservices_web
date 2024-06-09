using Algorithms.Domain.Core.Interfaces;

namespace Algorithms.Domain.Core;

public class TrackedAggregateRoot : AggregateRoot, IAudit, IDeleteable
{
    public DateTimeOffset AuditDtime { get; set; }
    public string AuditAuthor { get; set; }
    public bool IsActive { get; set; }

    public virtual void Delete()
    {
        IsActive = false;
    }
}