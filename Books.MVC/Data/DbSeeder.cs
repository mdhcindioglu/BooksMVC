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

            var adminUser = new User
            {
                FullName = Roles.Admin.ToUpper(),
                UserName = "admin@website.com",
                NormalizedUserName = "ADMIN@WEBSITE.COM",
                Email = "admin@website.com",
                NormalizedEmail = "ADMIN@WEBSITE.COM",
                EmailConfirmed = true,
                PhoneNumber = "+123456789",
                PhoneNumberConfirmed = true,
            };
            await userMngr!.CreateAsync(adminUser, "Password");

            await userMngr.AddToRoleAsync(adminUser, Roles.Admin);
        }

        if (await db!.Books.AnyAsync() == false)
        {
            if (await db.Categories.AnyAsync() == false)
            {
                var category1 = new Category { Name = "Category1", };
                var category2 = new Category { Name = "Category2", };
                await db.Categories.AddRangeAsync([category1, category2]);
                await db.SaveChangesAsync();

                var auther1 = new Auther() { FullName = "Auther1", };
                var auther2 = new Auther() { FullName = "Auther2", };
                await db.Authers.AddRangeAsync([auther1, auther2]);
                await db.SaveChangesAsync();

                var books = new List<Book>()
                {
                    new() { CategoryId = category1.Id, AutherId = auther1.Id, Title = "Book1", PublishDate = new DateTime(1995, 12, 25), AddedDate = DateTime.Now.AddDays(-120), Pages = 145, },
                    new() { CategoryId = category1.Id, AutherId = auther2.Id, Title = "Book2", PublishDate = new DateTime(2004, 1, 14), AddedDate = DateTime.Now.AddDays(-111), Pages = 564, },
                    new() { CategoryId = category2.Id, AutherId = auther1.Id, Title = "Book3", PublishDate = new DateTime(2010, 5, 16), AddedDate = DateTime.Now.AddDays(-91), Pages = 344, },
                    new() { CategoryId = category2.Id, AutherId = auther2.Id, Title = "Book4", PublishDate = new DateTime(2012, 2, 28), AddedDate = DateTime.Now.AddDays(-60), Pages = 195, },
                };
                await db.Books.AddRangeAsync(books);
                await db.SaveChangesAsync();
            }
        }
    }
}
