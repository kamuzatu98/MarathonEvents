using MarathonEvents.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ======================================================
// 1. SERVICES CONFIGURATION (Dependency Injection)
// ======================================================

// Add Controller support
builder.Services.AddControllers();

// Add EF Core DbContext with SQL Server connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Session and Distributed Memory Cache
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

// Register HttpContextAccessor for reading session data inside controllers
builder.Services.AddHttpContextAccessor();

// Configure Swagger / OpenAPI Generator
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ======================================================
// 2. MIDDLEWARE PIPELINE CONFIGURATION
// ======================================================

if (app.Environment.IsDevelopment())
{
    // Enable Swagger Middleware and UI
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RaceDay API v1");
        c.RoutePrefix = "swagger"; // Access UI at https://localhost:<port>/swagger
    });
}

app.UseHttpsRedirection();

// UseSession MUST be placed before UseAuthorization
app.UseSession();

app.UseAuthorization();

// Map Controller Routes
app.MapControllers();

// Run the Web Application
app.Run();