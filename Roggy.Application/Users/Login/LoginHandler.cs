using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Authentication;
using Roggy.Application.Abstractions.Persistence;

namespace Roggy.Application.Users.Login;

public sealed class LoginHandler
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginHandler(
        IApplicationDbContext dbContext,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _dbContext = dbContext;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<LoginResult> HandleAsync(
        LoginCommand command,
        CancellationToken cancellationToken = default)
    {
        var email = command.Email.Trim().ToLowerInvariant();

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(
                user => user.Email == email,
                cancellationToken);

        if (user is null)
        {
            throw new UnauthorizedAccessException(
                "Invalid email or password.");
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException(
                "This account is inactive.");
        }

        var passwordValid = BCrypt.Net.BCrypt.Verify(
            command.Password,
            user.PasswordHash);

        if (!passwordValid)
        {
            throw new UnauthorizedAccessException(
                "Invalid email or password.");
        }

        var token = _jwtTokenGenerator.GenerateToken(
            user.Id,
            user.Username,
            user.Email,
            user.Role);

        return new LoginResult(
            user.Id,
            user.Username,
            user.Email,
            user.DisplayName,
            token);
    }
}