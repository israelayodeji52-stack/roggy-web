using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Artists.SearchArtists;

public sealed class SearchArtistsHandler
{
    private readonly IApplicationDbContext _dbContext;

    public SearchArtistsHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<ArtistProfile>> HandleAsync(
        SearchArtistsQuery query,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query.Query))
        {
            return [];
        }

        var searchTerm = query.Query.Trim().ToLower();

        return await _dbContext.ArtistProfiles
            .AsNoTracking()
            .Where(artist =>
                artist.StageName.ToLower().Contains(searchTerm))
            .OrderBy(artist => artist.StageName)
            .ToListAsync(cancellationToken);
    }
}