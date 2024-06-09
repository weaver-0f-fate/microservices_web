namespace Algorithms.Domain.Core.User;

public class UserContext
{
    public string UserId { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    //public IEnumerable<Right>? Rights { get; private set; }
    public string GivenName { get; private set; } = default!;
    public string Surname { get; private set; } = default!;
    public string AccessToken { get; set; }

    public UserContext SetUserId(string userId)
    {
        UserId = userId;
        return this;
    }

    public UserContext SetName(string name)
    {
        Name = name;
        return this;
    }

    public UserContext SetGivenName(string givenName)
    {
        GivenName = givenName;
        return this;
    }

    public UserContext SetSurname(string surname)
    {
        Surname = surname;
        return this;
    }

    //public UserContext SetRights(IEnumerable<Right> rights)
    //{
    //    Rights = rights;
    //    return this;
    //}
}
