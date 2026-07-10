using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var dbType = Enum.Parse<DatabaseType>(
    Environment.GetEnvironmentVariable("DB_TYPE")
    ?? builder.Configuration["DatabaseType"]
);
var conn = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
    ?? builder.Configuration.GetConnectionString(dbType.ToString());
builder.Services.AddDbContext<AppDbContext>(DbContextFactory.Configure(dbType, conn));

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

builder.Services.AddScoped<AuthService>();

var app = builder.Build();

app.UseCors();
app.MapControllers();

app.Run("http://localhost:5000");
