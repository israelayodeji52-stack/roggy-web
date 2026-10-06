using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;

namespace Roggy.Application.Users.DeactivateUser;

public sealed class DeactivateUserHandler
{
    private readonly IApplicationDbContext _dbContext;

    public DeactivateUserHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(
        DeactivateUserCommand command,
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

        user.Deactivate();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}