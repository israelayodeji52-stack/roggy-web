using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Users.CreateUser;

public sealed class CreateUserHandler
{
    private readonly IApplicationDbContext _dbContext;

    public CreateUserHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> HandleAsync(
        CreateUserCommand command,
        CancellationToken cancellationToken = default)
    {
        var username = command.Username.Trim().ToLowerInvariant();
        var email = command.Email.Trim().ToLowerInvariant();
        var displayName = command.DisplayName.Trim();

        var usernameExists = await _dbContext.Users
            .AnyAsync(
                user => user.Username == username,
                cancellationToken);

        if (usernameExists)
        {
            throw new InvalidOperationException(
                "Username is already taken.");
        }

        var emailExists = await _dbContext.Users
            .AnyAsync(
                user => user.Email == email,
                cancellationToken);

        if (emailExists)
        {
            throw new InvalidOperationException(
                "An account with this email already exists.");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(
            command.Password);

        // Uses the domain factory method to instantiate encapsulated properties
        var user = Roggy.Domain.Entities.User.Create(
            username,
            email,
            passwordHash,
            displayName);

        _dbContext.Users.Add(user);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return user;
    }
}