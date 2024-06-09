namespace Algorithms.Infrastructure.Exceptions;

public class DomainEventsOverloadException : Exception
{
    public DomainEventsOverloadException(int countOfEvents, int limit) : base($"Surpassed the limit of {limit} publishable domain events with count of {countOfEvents}"){}
}