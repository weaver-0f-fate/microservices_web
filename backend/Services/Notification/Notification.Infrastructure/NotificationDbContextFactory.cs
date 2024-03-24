using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Notification.Infrastructure;

public class NotificationDbContextFactory : IDesignTimeDbContextFactory<NotificationDbContext>
{
    private readonly IConfiguration _configuration;

    public NotificationDbContextFactory()
    {
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory());

        _configuration = configurationBuilder.Build();
    }

    public NotificationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<NotificationDbContext>();
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));

        return new NotificationDbContext(optionsBuilder.Options);
    }
}