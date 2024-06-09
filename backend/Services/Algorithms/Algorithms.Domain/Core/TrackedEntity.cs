using Algorithms.Domain.Core.Interfaces;

namespace Algorithms.Domain.Core;

public class TrackedEntity : Entity, IAudit, IDeleteable
{
    public DateTimeOffset AuditDtime { get; set; }
    public string AuditAuthor { get; set; }
    public bool IsActive { get; set; }

    public void Delete()
    {
        IsActive = false;
    }
}
