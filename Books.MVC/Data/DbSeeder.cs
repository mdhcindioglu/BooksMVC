using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Books.MVC.Data;


public static class WebApplicationExtensions
{
    public static async Task DbSeeder(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var db = scope.ServiceProvider.GetService<MainDbContext>();
        using var userMngr = scope.ServiceProvider.GetService<UserManager<User>>();
        using var roleMngr = scope.ServiceProvider.GetService<RoleManager<Role>>();

        var adminRole = roleMngr!.Roles.FirstOrDefault(x => x.Name == Roles.Admin);
        if (adminRole == null)
        {
            adminRole = new Role { Name = Roles.Admin, ConcurrencyStamp = Guid.NewGuid().ToString().ToUpper(), };
            await roleMngr.CreateAsync(adminRole);
            
            var adminUser = new User {
                UserName = "admin@website.com",
                NormalizedUserName = "ADMIN@WEBSITE.COM",
                Email = "admin@website.com",
                NormalizedEmail = "ADMIN@WEBSITE.COM",
                EmailConfirmed = true,
                FullName = Roles.Admin.ToUpper(),
            };
            await userMngr!.CreateAsync(adminUser, "$Admin1$");

            await userMngr.AddToRoleAsync(adminUser, Roles.Admin);
        }

    }
}
