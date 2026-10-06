using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;

namespace Roggy.Application.Artists.GetArtistFollowStatus;

public sealed class GetArtistFollowStatusHandler
{
    private readonly IApplicationDbContext _dbContext;

    public GetArtistFollowStatusHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> HandleAsync(
        GetArtistFollowStatusQuery query,
        CancellationToken cancellationToken = default)
    {
        if (query.UserId == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(query.UserId));
        }

        if (query.ArtistId == Guid.Empty)
        {
            throw new ArgumentException(
                "Artist ID is required.",
                nameof(query.ArtistId));
        }

        var artistExists = await _dbContext.ArtistProfiles
            .AnyAsync(
                artist => artist.Id == query.ArtistId,
                cancellationToken);

        if (!artistExists)
        {
            throw new KeyNotFoundException(
                "Artist profile was not found.");
        }

        return await _dbContext.ArtistFollows
            .AnyAsync(
                follow =>
                    follow.UserId == query.UserId &&
                    follow.ArtistId == query.ArtistId,
                cancellationToken);
    }
}