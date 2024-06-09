using Algorithms.Domain.Core.Interfaces;

namespace Algorithms.Domain.Core;

public class TrackedEntity : EntityWithUuid, IAudit, IDeleteable
{
    public DateTimeOffset AuditDtime { get; set; }
    public string AuditAuthor { get; set; }
    public bool IsActive { get; set; }

    public void Delete()
    {
        IsActive = false;
    }
}
