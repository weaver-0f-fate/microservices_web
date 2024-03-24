using Subscription.Domain.Aggregates;

namespace Subscription.Domain.Tests.Aggregates;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

[TestClass]
public class SubscriptionTests
{
    [TestMethod]
    public void Subscription_Constructor_InitializesProperties()
    {
        // Arrange
        var user = new User("test@example.com");
        var eventUuid = Guid.NewGuid();

        // Act
        var subscription = new Domain.Aggregates.Subscription(user, eventUuid);

        // Assert
        Assert.IsNotNull(subscription.Id);
        Assert.AreEqual(user, subscription.User);
        Assert.AreEqual(eventUuid, subscription.EventUuid);
    }

    [TestMethod]
    public void Subscription_Id_IsGuid()
    {
        // Arrange
        var user = new User("test@example.com");
        var eventUuid = Guid.NewGuid();

        // Act
        var subscription = new Domain.Aggregates.Subscription(user, eventUuid);

        // Assert
        Assert.IsTrue(Guid.TryParse(subscription.Id.ToString(), out _));
    }

    [TestMethod]
    public void Subscription_SetUser_UpdatesUserProperty()
    {
        // Arrange
        var user = new User("old@example.com");
        var eventUuid = Guid.NewGuid();
        var subscription = new Domain.Aggregates.Subscription(user, eventUuid);

        var newUser = new User("new@example.com");

        // Act
        subscription.User = newUser;

        // Assert
        Assert.AreEqual(newUser, subscription.User);
    }
}
