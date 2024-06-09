using Algorithms.Domain.Core;

namespace Algorithms.Domain.Aggregates.AlgorithmAggregate;

internal class Stage : TrackedEntity
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public Algorithm Algorithm { get; private set; }

    private Stage() : base() { }

    public Stage(Algorithm algorithm, string name) : this()
    {
        Name = name;
        Algorithm = algorithm;
    }
}
