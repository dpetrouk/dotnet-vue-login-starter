using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using backend.Data;
using backend.Services;

var builder = WebApplication.CreateBuilder(args);

var rawDbType = Environment.GetEnvironmentVariable("DB_TYPE")
    ?? builder.Configuration["DatabaseType"]
    ?? throw new InvalidOperationException(
        $"DatabaseType not configured. Set DB_TYPE env var or add 'DatabaseType' to appsettings.json. " +
        $"Use one of: {string.Join(", ", Enum.GetNames<DatabaseType>())} (or add new)"
    );
if (!Enum.TryParse<DatabaseType>(rawDbType, out var dbType))
    throw new InvalidOperationException(
        $"Unknown DatabaseType '{rawDbType}'. " +
        $"Use one of: {string.Join(", ", Enum.GetNames<DatabaseType>())} (or add new)"
    );

Console.WriteLine($"Using database: {dbType}");
var conn = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
    ?? builder.Configuration.GetConnectionString(dbType.ToString())
    ?? throw new InvalidOperationException(
        $"Connection string not found for '{dbType}'. Set DB_CONNECTION_STRING env var or add a matching key in ConnectionStrings."
    );

var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET")
    ?? jwtSection["Secret"]
    ?? throw new InvalidOperationException("Jwt:Secret is not configured. Set JWT_SECRET env var or add Jwt:Secret in appsettings.Development.json.");

builder.Services.Configure<JwtOptions>(jwtSection);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddDbContext<AppDbContext>(DbContextFactory.Configure(dbType, conn));
builder.Services.AddControllers();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ProfileService>();

// CORS для Vue dev server
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors();
app.MapControllers();

app.Run("http://localhost:5000");
