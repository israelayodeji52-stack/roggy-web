using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Artists.GetArtists;

public sealed class GetArtistsHandler
{
    private readonly IApplicationDbContext _dbContext;

    public GetArtistsHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<ArtistProfile>> HandleAsync(
        GetArtistsQuery query,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.ArtistProfiles
            .AsNoTracking()
            .OrderBy(artist => artist.StageName)
            .ToListAsync(cancellationToken);
    }
}