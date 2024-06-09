namespace Algorithms.Domain.Core.Interfaces;

public interface IDeleteable
{
    public bool IsActive { get; set; }
    public void Delete();
}
