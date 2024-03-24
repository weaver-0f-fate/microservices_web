using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Subscription.Infrastructure;

public class SubscriptionDbContextFactory : IDesignTimeDbContextFactory<SubscriptionDbContext>
{
    private readonly IConfiguration _configuration;

    public SubscriptionDbContextFactory()
    {
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory());

        _configuration = configurationBuilder.Build();
    }

    public SubscriptionDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SubscriptionDbContext>();
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));

        return new SubscriptionDbContext(optionsBuilder.Options);
    }
}