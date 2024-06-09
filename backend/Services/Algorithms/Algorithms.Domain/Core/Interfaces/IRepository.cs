
namespace Algorithms.Domain.Core.Interfaces;
public interface IRepository
{
    IUnitOfWork UnitOfWork { get; }
}