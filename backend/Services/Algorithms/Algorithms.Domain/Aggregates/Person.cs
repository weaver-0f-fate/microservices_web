using Algorithms.Domain.Core;

namespace Algorithms.Domain.Aggregates;

public class Person : TrackedEntity
{
    public string Name { get; set; }
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string Email { get; set; }

    private Person() { }

    public Person(Guid uuid, string name, string address1, string address2, string city, string country, string email) : base()
    {
        Uuid = uuid;
        Name = name;
        Address1 = address1;
        Address2 = address2;
        City = city;
        Country = country;
        Email = email;
        AuditAuthor = "System";
        AuditDtime = DateTimeOffset.UtcNow;
        IsActive = true;
    }
}

