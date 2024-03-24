namespace Subscription.Domain.Tests.Aggregates;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subscription.Domain.Aggregates;
using System;

[TestClass]
public class UserTests
{
    [TestMethod]
    public void User_Constructor_InitializesProperties()
    {
        // Arrange
        var email = "test@example.com";

        // Act
        var user = new User(email);

        // Assert
        Assert.IsNotNull(user.Id);
        Assert.AreEqual(email, user.Email);
    }

    [TestMethod]
    public void User_Id_IsGuid()
    {
        // Arrange
        var email = "test@example.com";

        // Act
        var user = new User(email);

        // Assert
        Assert.IsTrue(Guid.TryParse(user.Id.ToString(), out _));
    }

    [TestMethod]
    public void User_SetEmail_UpdatesEmailProperty()
    {
        // Arrange
        var user = new User("old@example.com");
        var newEmail = "new@example.com";

        // Act
        user.Email = newEmail;

        // Assert
        Assert.AreEqual(newEmail, user.Email);
    }
}
