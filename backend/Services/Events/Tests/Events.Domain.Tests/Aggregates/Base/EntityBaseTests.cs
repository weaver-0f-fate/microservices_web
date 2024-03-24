using Events.Domain.Aggregates.Base;

namespace Events.Domain.Tests.Aggregates.Base;

[TestClass]
public class EntityBaseTests
{
    [TestMethod]
    public void EntityBase_IdProperty_ShouldGetAndSetCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity = new MockEntity
        {
            // Act
            Id = id
        };

        // Assert
        Assert.AreEqual(id, entity.Id);
    }
}

public class MockEntity : EntityBase { }