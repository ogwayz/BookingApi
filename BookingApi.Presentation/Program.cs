using BookingApi.Aplication.Services;
using BookingApi.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;
using DotNetEnv;
using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.Load("../.env");
System.Console.WriteLine(Environment.GetEnvironmentVariable("ADMIN_EMAIL"));
System.Console.WriteLine(Environment.GetEnvironmentVariable("ADMIN_PASSWORD"));

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

builder.Services.AddControllers();

var connectionString = string.Format(
    "Host={0};Port={1};Database={2};User Id={3};Password={4}",
    Environment.GetEnvironmentVariable("DB_HOST"),
    Environment.GetEnvironmentVariable("DB_PORT"),
    Environment.GetEnvironmentVariable("DB_NAME"),
    Environment.GetEnvironmentVariable("DB_USER"),
    Environment.GetEnvironmentVariable("DB_PASSWORD"));

Console.WriteLine($"Connection String: {connectionString}");

builder.Services.AddDbContext<BookingDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<RoomRepository>();
builder.Services.AddScoped<BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<SeedService>();


builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;


})
.AddEntityFrameworkStores<BookingDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                System.Console.WriteLine($"[DEBUG] JWT Authentication Failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                System.Console.WriteLine($"[DEBUG] JWT Token Validated: {context.Principal?.Identity?.Name}");
                return Task.CompletedTask;
            }
        };

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
Console.WriteLine("Applying migrations...");
await dbContext.Database.MigrateAsync();
var seedService = scope.ServiceProvider.GetRequiredService<SeedService>();
await seedService.SeedAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Use(async (context, next) =>
{
    System.Console.WriteLine($"[DEBUG] Request: {context.Request.Path} Method: {context.Request.Method} Auth: {context.Request.Headers["Authorization"]} Status: {context.Response.StatusCode}");
    await next.Invoke();
    System.Console.WriteLine($"[DEBUG] Response: {context.Request.Path} Status: {context.Response.StatusCode}");
});
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
