using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using backend.Data;

namespace backend.Services;

public class AuthService
{
    private readonly PostgresDbContext _postgres;
    private readonly SqliteDbContext _sqlite;

    public AuthService(PostgresDbContext postgres, SqliteDbContext sqlite)
    {
        _postgres = postgres;
        _sqlite = sqlite;
    }

    public async Task<UserProfileDto?> LoginAsync(string email, string password)
    {
        // 1. Проверяем логин в PostgreSQL
        var user = await _postgres.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
            return null;

        var hasher = new PasswordHasher<object>();
        var result = hasher.VerifyHashedPassword(new object(), user.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed)
            return null;

        // 2. Достаём профиль из SQLite
        var profile = await _sqlite.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == user.Id);

        return new UserProfileDto
        {
            UserId = user.Id,
            Email = user.Email,
            FullName = profile?.FullName ?? "—",
            Profile = profile != null
                ? new ProfileDto
                {
                    City = profile.City,
                    Street = profile.Street,
                    Building = profile.Building,
                    ZipCode = profile.ZipCode
                }
                : null
        };
    }
}

public record UserProfileDto
{
    public int UserId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public ProfileDto? Profile { get; init; }
}

public record ProfileDto
{
    public string City { get; init; } = string.Empty;
    public string Street { get; init; } = string.Empty;
    public string Building { get; init; } = string.Empty;
    public string ZipCode { get; init; } = string.Empty;
}
