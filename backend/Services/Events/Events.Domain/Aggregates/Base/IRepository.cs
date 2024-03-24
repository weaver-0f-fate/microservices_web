using Ardalis.Specification;

namespace Events.Domain.Aggregates.Base;

public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot { }