using Ardalis.Specification;

namespace Subscription.Domain.Aggregates.Base;

public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot { }