using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Artists.FollowArtist;

public sealed class FollowArtistHandler
{
    private readonly IApplicationDbContext _dbContext;

    public FollowArtistHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ArtistFollow> HandleAsync(
        FollowArtistCommand command,
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

        var artist = await _dbContext.ArtistProfiles
            .FirstOrDefaultAsync(
                artist => artist.Id == command.ArtistId,
                cancellationToken);

        if (artist is null)
        {
            throw new KeyNotFoundException(
                "Artist profile was not found.");
        }

        var existingFollow = await _dbContext.ArtistFollows
            .FirstOrDefaultAsync(
                follow =>
                    follow.UserId == command.UserId &&
                    follow.ArtistId == command.ArtistId,
                cancellationToken);

        if (existingFollow is not null)
        {
            throw new InvalidOperationException(
                "You are already following this artist.");
        }

        var follow = new ArtistFollow(
            command.UserId,
            command.ArtistId);

        _dbContext.ArtistFollows.Add(follow);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return follow;
    }
}