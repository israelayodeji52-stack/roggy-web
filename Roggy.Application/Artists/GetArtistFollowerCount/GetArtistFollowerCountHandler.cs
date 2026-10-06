using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;

namespace Roggy.Application.Artists.GetArtistFollowerCount;

public sealed class GetArtistFollowerCountHandler
{
    private readonly IApplicationDbContext _dbContext;

    public GetArtistFollowerCountHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> HandleAsync(
        GetArtistFollowerCountQuery query,
        CancellationToken cancellationToken = default)
    {
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
            .CountAsync(
                follow => follow.ArtistId == query.ArtistId,
                cancellationToken);
    }
}