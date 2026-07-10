using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using backend.Data;

namespace backend.Services;

public class AuthService
{
    private readonly AppDbContext _appDbContext;

    public AuthService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<UserProfileDto?> LoginAsync(string email, string password)
    {
        // 1. Проверяем логин
        var user = await _appDbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
            return null;

        var hasher = new PasswordHasher<object>();
        var result = hasher.VerifyHashedPassword(new object(), user.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed)
            return null;

        // 2. Достаём профиль
        var profile = await _appDbContext.UserProfiles
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
