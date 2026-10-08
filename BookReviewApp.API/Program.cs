using BookReviewApp.Business.Interfaces;
using BookReviewApp.Business.Services;
using BookReviewApp.DataAccess;
using BookReviewApp.DataAccess.Interfaces;
using BookReviewApp.DataAccess.Pagination;
using BookReviewApp.DataAccess.Repositories;
using BookReviewApp.Entities.Models;
using BookReviewApp.Infrastructure.Middleware;
using BookReviewApp.WebUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri("https://localhost:7141/");
});
var connectionString = Environment.GetEnvironmentVariable("BOOKREVIEW_DB_CONNECTION");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString ??
                      throw new InvalidOperationException("DB connection string not set")));


builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IReviewVoteRepository, ReviewVoteRepository>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddSingleton<IBookCursorCodec, BookCursorCodec>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookReviewApp API", Version = "v1" });

    // Include XML comments if you generate them
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);

    // (Optional) configure JWT/security here if needed
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    // --- Swagger middleware (Development only) ---
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookReviewApp API v1");
        c.RoutePrefix = "swagger"; // default; keeps URL at /swagger/index.html
    });
    // --- end Swagger middleware ---
}

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

app.MapRazorPages();
app.Run();
