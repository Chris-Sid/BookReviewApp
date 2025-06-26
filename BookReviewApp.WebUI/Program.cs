using BookReviewApp.Business.Interfaces;
using BookReviewApp.Business.Services;
using BookReviewApp.DataAccess;
using BookReviewApp.DataAccess.Interfaces;
using BookReviewApp.DataAccess.Repositories;
using BookReviewApp.Entities.Models;
using BookReviewApp.Infrastructure.Middleware;
using BookReviewApp.WebUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri("https://localhost:7141/");
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IReviewVoteRepository, ReviewVoteRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Account/AccessDenied";
});
var app = builder.Build();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Books}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    await SeedData.InitializeAsync(app.Services);
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Shows full error with stacktrace
}
else
{
    // For Production – Use error boundary
    app.UseExceptionHandler("/Home/Error"); // Redirects on unhandled exceptions
    app.UseHsts();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Books}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();
