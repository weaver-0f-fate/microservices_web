using AutoMapper;

namespace Events.Application.Core.Tests.Profiles;

public abstract class BaseProfilesTest<T> where T : Profile
{
    private readonly List<Type> _profiles = new();

    protected BaseProfilesTest() { }

    /// <summary>
    /// </summary>
    /// <param name="profile">List of Profiles required to check main Profile.</param>
    protected BaseProfilesTest(params Type[] profile)
    {
        _profiles.AddRange(profile);
    }

    [TestMethod]
    public void AppCoreProfiles_ShouldValidateMappingProfiles()
    {
        // Arrange
        var config = new MapperConfiguration(configure =>
        {
            configure.AddProfile(typeof(T));
            foreach (var item in _profiles)
            {
                configure.AddProfile(item);
            }
        });

        // Act
        var mapper = config.CreateMapper();

        // Assert
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}