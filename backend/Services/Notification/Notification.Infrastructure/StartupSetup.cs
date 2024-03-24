using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Notification.Infrastructure;

public static class StartupSetup
{
    public static async Task AddDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<NotificationDbContext>(options =>
            options.UseSqlServer(connectionString));

        await using var serviceProvider = services.BuildServiceProvider();
        var context = serviceProvider.GetRequiredService<NotificationDbContext>();
        await context.Database.MigrateAsync();
    }
}