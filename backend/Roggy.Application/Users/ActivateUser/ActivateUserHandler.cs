using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;

namespace Roggy.Application.Users.ActivateUser;

public sealed class ActivateUserHandler
{
    private readonly IApplicationDbContext _dbContext;

    public ActivateUserHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(
        ActivateUserCommand command,
        CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(
                user => user.Id == command.UserId,
                cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException(
                "User was not found.");
        }

        user.Activate();

        await _dbContext.SaveChangesAsync(
            cancellationToken);
    }
}