using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Artists.GetFollowedArtists;

public sealed class GetFollowedArtistsHandler
{
    private readonly IApplicationDbContext _dbContext;

    public GetFollowedArtistsHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<ArtistProfile>> HandleAsync(
        GetFollowedArtistsQuery query,
        CancellationToken cancellationToken = default)
    {
        if (query.UserId == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(query.UserId));
        }

        if (query.Limit <= 0)
        {
            throw new ArgumentException(
                "Limit must be greater than zero.",
                nameof(query.Limit));
        }

        var limit = Math.Min(query.Limit, 100);

        return await _dbContext.ArtistFollows
            .Where(follow => follow.UserId == query.UserId)
            .OrderByDescending(follow => follow.CreatedAt)
            .Take(limit)
            .Select(follow => follow.Artist)
            .ToListAsync(cancellationToken);
    }
}