global using Books.MVC.Data;
global using Books.MVC.Data.Entities;
using Books.MVC.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var CS = builder.Configuration.GetConnectionString("CS") ?? throw new InvalidOperationException("Connection string 'CS' not found.");
builder.Services.AddDbContext<MainDbContext>(options => options.UseSqlServer(CS));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<User, Role>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<MainDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddMvc();

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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
