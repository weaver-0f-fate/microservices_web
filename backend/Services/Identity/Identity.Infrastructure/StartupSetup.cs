using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Identity.Infrastructure;

public static class StartupSetup
{
    public static async Task AddDbContext(this IServiceCollection services, string connectionString, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString, opt =>
            {
                opt.EnableRetryOnFailure();
            }));

        services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddUserManager<UserManager<IdentityUser>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication().AddIdentityServerJwt();

        await using var serviceProvider = services.BuildServiceProvider();

        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.MigrateAsync();
        await CreateRoles(serviceProvider);
    }

    private static async Task CreateRoles(IServiceProvider serviceProvider)
    {

        //adding custom roles
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleNames = new List<string> { "Admin", "Instructor", "User", "Guest" };

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        //creating a super user who could maintain the web app
        var poweruser = new IdentityUser
        {
            UserName = configuration.GetSection("AppSettings")["UserEmail"],
            Email = configuration.GetSection("AppSettings")["UserEmail"]
        };

        var userPassword = configuration.GetSection("AppSettings")["UserPassword"];
        var user = await userManager.FindByEmailAsync(configuration.GetSection("AppSettings")["UserEmail"]);

        if (user == null)
        {
            var createPowerUser = await userManager.CreateAsync(poweruser, userPassword);
            if (createPowerUser.Succeeded)
            {
                //here we tie the new user to the "Admin" role 
                await userManager.AddToRoleAsync(poweruser, "Admin");

            }
        }
    }
}