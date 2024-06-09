namespace Algorithms.Domain.Core.Interfaces;

public interface IProvider<in TIn, out TOut>
{
#pragma warning disable CA1716 // Identifiers should not match keywords
    TOut Get(TIn input);
#pragma warning restore CA1716 // Identifiers should not match keywords
}

public interface IProvider<out TOut>
{
#pragma warning disable CA1716 // Identifiers should not match keywords
    TOut Get();
#pragma warning restore CA1716 // Identifiers should not match keywords
}