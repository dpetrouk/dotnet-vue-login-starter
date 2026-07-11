using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Data;

namespace backend.Services;

public class AuthService
{
    private readonly AppDbContext _appDbContext;
    private readonly IOptions<JwtOptions> _jwtOptions;

    public AuthService(AppDbContext appDbContext, IOptions<JwtOptions> jwtOptions)
    {
        _appDbContext = appDbContext;
        _jwtOptions = jwtOptions;
    }

    public string GenerateToken(int userId)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<int?> LoginAsync(string email, string password)
    {
        var user = await _appDbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
            return null;

        var hasher = new PasswordHasher<object>();
        var result = hasher.VerifyHashedPassword(new object(), user.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed)
            return null;

        return user.Id;
    }
}

public class JwtOptions
{
    public string Secret { get; set; } = string.Empty;
}
