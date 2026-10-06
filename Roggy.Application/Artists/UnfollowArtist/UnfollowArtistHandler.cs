using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;

namespace Roggy.Application.Artists.UnfollowArtist;

public sealed class UnfollowArtistHandler
{
    private readonly IApplicationDbContext _dbContext;

    public UnfollowArtistHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(
        UnfollowArtistCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.UserId == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(command.UserId));
        }

        if (command.ArtistId == Guid.Empty)
        {
            throw new ArgumentException(
                "Artist ID is required.",
                nameof(command.ArtistId));
        }

        var follow = await _dbContext.ArtistFollows
            .FirstOrDefaultAsync(
                item =>
                    item.UserId == command.UserId &&
                    item.ArtistId == command.ArtistId,
                cancellationToken);

        if (follow is null)
        {
            throw new KeyNotFoundException(
                "You are not following this artist.");
        }

        _dbContext.ArtistFollows.Remove(follow);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}