using Algorithms.Domain.Core;

namespace Algorithms.Domain.Aggregates.AlgorithmAggregate;

public class Algorithm : TrackedAggregateRoot
{
    public string Name { get; set; }

    private List<Stage> _stages = new();

    public Algorithm(string name)
    {
        Name = name;
    }

    public void AddStage(string stageName)
    {
        var newStage = new Stage(this, stageName);
        _stages.Add(newStage);
    }
}

