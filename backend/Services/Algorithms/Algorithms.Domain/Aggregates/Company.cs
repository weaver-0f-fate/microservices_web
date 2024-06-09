using Algorithms.Domain.Core;


namespace Algorithms.Domain.Aggregates;

public class Company : TrackedEntity
{
    public string Name { get; set; }
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string City { get; set; }

}
