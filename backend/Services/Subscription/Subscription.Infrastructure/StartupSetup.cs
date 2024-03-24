using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Subscription.Infrastructure;

public static class StartupSetup
{
    public static async Task AddDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<SubscriptionDbContext>(options =>
            options.UseSqlServer(connectionString));

        await using var serviceProvider = services.BuildServiceProvider();
        var context = serviceProvider.GetRequiredService<SubscriptionDbContext>();
        await context.Database.MigrateAsync();
    }
}