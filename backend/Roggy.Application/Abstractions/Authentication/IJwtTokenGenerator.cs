namespace Roggy.Application.Abstractions.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(
        Guid userId,
        string username,
        string email,
        string role);
}