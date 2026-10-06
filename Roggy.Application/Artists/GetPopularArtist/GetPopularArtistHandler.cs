using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Artists.GetPopularArtists;

public sealed class GetPopularArtistsHandler
{
    private readonly IApplicationDbContext _dbContext;

    public GetPopularArtistsHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<ArtistProfile>> HandleAsync(
        GetPopularArtistsQuery query,
        CancellationToken cancellationToken = default)
    {
        var limit = query.Limit <= 0
            ? 20
            : Math.Min(query.Limit, 100);

        return await _dbContext.ArtistProfiles
            .AsNoTracking()
            .Where(artist =>
                artist.User.IsActive &&
                artist.IsVerified)
            .OrderByDescending(artist =>
                _dbContext.Songs
                    .Where(song => song.ArtistId == artist.Id)
                    .Sum(song => (long?)song.PlayCount) ?? 0)
            .ThenByDescending(artist => artist.CreatedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
}