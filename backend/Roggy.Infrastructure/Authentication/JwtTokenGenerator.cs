using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Roggy.Application.Abstractions.Authentication;

namespace Roggy.Infrastructure.Authentication;

public sealed class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(
        Guid userId,
        string username,
        string email,
        string role)
    {
        var secret = _configuration["Jwt:Secret"]
            ?? throw new InvalidOperationException(
                "JWT secret is not configured.");

        if (Encoding.UTF8.GetByteCount(secret) < 32)
        {
            throw new InvalidOperationException(
                "JWT secret must be at least 32 bytes long.");
        }

        var issuer = _configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException(
                "JWT issuer is not configured.");

        var audience = _configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException(
                "JWT audience is not configured.");

        var expirationMinutesValue =
            _configuration["Jwt:ExpirationMinutes"];

        var expirationMinutes = 60;

        if (!string.IsNullOrWhiteSpace(expirationMinutesValue))
        {
            if (!int.TryParse(
                    expirationMinutesValue,
                    out expirationMinutes))
            {
                throw new InvalidOperationException(
                    "JWT expiration minutes must be a valid integer.");
            }
        }

        if (expirationMinutes <= 0)
        {
            throw new InvalidOperationException(
                "JWT expiration minutes must be greater than zero.");
        }

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Name, username),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.Role, role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secret));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}