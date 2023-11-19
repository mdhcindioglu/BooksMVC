global using Books.MVC.Data;
global using Books.MVC.Data.Entities;
global using Books.MVC.Data.ViewModels;
global using Microsoft.AspNetCore.Authorization;

using Books.MVC.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var CS = builder.Configuration.GetConnectionString("CS") ?? throw new InvalidOperationException("Connection string 'CS' not found.");
        builder.Services.AddDbContext<MainDbContext>(options => options.UseSqlServer(CS));

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddIdentity<User, Role>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;

            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
        }).AddEntityFrameworkStores<MainDbContext>();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "http://127.0.0.1:8021";
                options.RequireHttpsMetadata = false;
                options.Audience = "ServiceA";
            });
        builder.Services.AddControllersWithViews();
        builder.Services.AddMvc();
        builder.Services.AddAutoMapper(typeof(Program).Assembly);
        builder.Services.AddSingleton<IEmailSender, EmailSender>();
        builder.Services.AddSingleton<IUserEmailStore<User>, UserEmailStore>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        using var scope = app.Services.CreateScope();
        using var db = scope.ServiceProvider.GetService<MainDbContext>();
        if (db!.Database.GetPendingMigrations().Count() > 0)
            db.Database.Migrate();

        await app.DbSeeder();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.MapRazorPages();

        app.Run();
    }
}