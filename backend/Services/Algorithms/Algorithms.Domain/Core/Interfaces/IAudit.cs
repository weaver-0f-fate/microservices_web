namespace Algorithms.Domain.Core.Interfaces;

public interface IAudit
{
    public DateTimeOffset AuditDtime { get; set; }
    public string AuditAuthor { get; set; }
}