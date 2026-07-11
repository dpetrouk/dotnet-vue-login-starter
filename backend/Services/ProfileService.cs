using Microsoft.EntityFrameworkCore;
using backend.Data;

namespace backend.Services;

public class ProfileService
{
    private readonly AppDbContext _appDbContext;

    public ProfileService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<UserProfileDto?> GetProfileAsync(int userId)
    {
        var user = await _appDbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return null;

        var profile = await _appDbContext.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId);

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
