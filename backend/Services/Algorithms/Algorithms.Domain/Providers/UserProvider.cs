using Microsoft.Extensions.Options;
using Algorithms.Domain.Core.User;
using Algorithms.Domain.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;

namespace Algorithms.Domain.Providers;

public interface IUserProvider : IProvider<User> { }

public class User
{
    public string Id { get; set; } = default!;
    public string Eesnimi { get; set; } = default!;
    public string Perenimi { get; set; } = default!;
}

public class UserProvider : IUserProvider
{
    private readonly UserContext _userContext;
    private readonly IKeycloakUserClient _keycloakUserClient;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<UserProvider> _logger;

    private readonly MemoryCacheEntryOptions _rolesCacheEntryOptions;

    public UserProvider(UserContext userContext,
        IKeycloakUserClient keycloakUserClient,
        IMemoryCache memoryCache,
        IOptions<UserConfig> config,
        ILogger<UserProvider> logger)
    {
        _userContext = userContext;
        _keycloakUserClient = keycloakUserClient;
        _memoryCache = memoryCache;
        _logger = logger;

        _rolesCacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(config.Value.RolesCacheMinutes));
    }

    public User Get()
    {
        return new User
        {
            Id = _userContext.UserId,
            Eesnimi = _userContext.GivenName,
            Perenimi = _userContext.Surname,
            //Masstransit kaudu tulevad kohe õigused
            //TODO (18.04.2024 Sander Baikov): Õiguste saatmise asemel peaks message busi panema kasutaja bearer tokeni kaasa
            //ja selle alusel autentida ning autoriseerida nagu otse tehtud rest api päringute puhul.
            //Küll aga siis tekib küsimus, mida süsteemsete sõnumitega teha...
            //Roles = _userContext.Rights != null
            //    ? _userContext.Rights
            //    : GetRoles()
        };
    }

    //private IEnumerable<Right> GetRoles()
    //{
    //    var cacheKey = CacheHelper.GetUserRolesCacheKey(_userContext.UserId);
    //    var cachedRights = _memoryCache.Get<Right[]>(cacheKey);

    //    if (cachedRights != null)
    //    {
    //        _logger.LogDebug("Roles cache - Found cache for {id}", cacheKey);
    //        return cachedRights;
    //    }

    //    _logger.LogDebug("Roles cache - No cache for {id}", cacheKey);

    //    var userInfo = _keycloakUserClient
    //        .GetUserInfo(_userContext.AccessToken)
    //        .GetAwaiter()
    //        .GetResult();

    //    var roles = AuthHelper.MapFromIdentifiers(userInfo.Roles).ToArray();

    //    _memoryCache.Set(cacheKey, roles, _rolesCacheEntryOptions);
    //    _logger.LogDebug("Roles cache - added to cache for {id}", cacheKey);

    //    return roles;
    //}
}