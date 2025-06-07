using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTFGym.Data;
using PTFGym.Hubs;
using PTFGym.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configure Identity with custom ApplicationUser and roles
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // Optional: Set to true if you need email confirmation
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();  // Ensure this is included

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add the controllers with views and remove antiforgery token validation
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Remove(new AutoValidateAntiforgeryTokenAttribute());
});

builder.Services.AddRazorPages(options =>
{
    // Custom route for Login page
    options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "/login");
    options.Conventions.AddAreaPageRoute("Identity", "/Account/Logout", "/logout");
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add SignalR service
builder.Services.AddSignalR(); // Moved before builder.Build()

// Build the app
var app = builder.Build();

// Map the SignalR hub
app.MapHub<ChatHub>("/chatHub"); // This is fine here

// Seed Roles and Admin User
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await SeedData.Initialize(services);  // Make sure SeedData.Initialize works correctly
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database: {Message}", ex.Message);
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();  // Ensure this is here
app.UseAuthorization();   // Ensure this is here

// Define routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Ensure Razor Pages are correctly mapped

app.Run();