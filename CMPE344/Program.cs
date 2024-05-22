using CMPE344.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

Console.WriteLine("Enter root password: ");

// Prompt the user to enter the root password for the database.
Database.ROOT_PASSWORD = Console.ReadLine()!;

var builder = WebApplication.CreateBuilder(args);

// Configure services for the application
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Set the name of the authentication cookie
        options.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme;

        // Enhance security by setting HttpOnly to true
        options.Cookie.HttpOnly = true;

        // Set the cookie expiration time to 1 hour
        options.ExpireTimeSpan = TimeSpan.FromHours(1);

        // Define the paths for login and access denied redirects
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";

        // Enable sliding expiration to refresh the cookie expiration time
        options.SlidingExpiration = true;
    });

// Register the database service as a singleton for dependency injection
builder.Services.AddSingleton<IDatabase, Database>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add support for Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Enable serving static files
app.UseStaticFiles();

// Enable routing for incoming requests
app.UseRouting();

// Enable authorization middleware
app.UseAuthorization();

// Map Razor Pages to their routes
app.MapRazorPages();

// Map default controller routes with a pattern
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Start the application
app.Run();