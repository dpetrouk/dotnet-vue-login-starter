using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// PostgreSQL — основной контекст (пользователи)
var pgConn = Environment.GetEnvironmentVariable("PG_CONNECTION_STRING")
    ?? builder.Configuration.GetConnectionString("Postgres")
    ?? "Host=localhost;Database=testdb;Username=postgres;Password=postgres";

builder.Services.AddDbContext<PostgresDbContext>(options =>
    options.UseNpgsql(pgConn));

// SQLite — профили
var sqliteConn = Environment.GetEnvironmentVariable("SQLITE_CONNECTION_STRING")
    ?? builder.Configuration.GetConnectionString("Sqlite")
    ?? "Data Source=profiles.db";

builder.Services.AddDbContext<SqliteDbContext>(options =>
    options.UseSqlite(sqliteConn));

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

// EnsureCreated — создаёт таблицы при первом запуске (пустые)
// Сид-данные добавляются отдельными SQL-скриптами:
//   PostgreSQL: sql/postgres/seed.sql
//   SQLite:     sql/sqlite/seed.sql
using (var scope = app.Services.CreateScope())
{
    var pg = scope.ServiceProvider.GetRequiredService<PostgresDbContext>();
    pg.Database.EnsureCreated();
}
using (var scope = app.Services.CreateScope())
{
    var sqlite = scope.ServiceProvider.GetRequiredService<SqliteDbContext>();
    sqlite.Database.EnsureCreated();
}

app.Run("http://localhost:5000");
