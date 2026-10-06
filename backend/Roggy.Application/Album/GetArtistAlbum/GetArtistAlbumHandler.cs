using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Albums.GetArtistAlbums;

public sealed class GetArtistAlbumsHandler
{
    private readonly IApplicationDbContext _dbContext;

    public GetArtistAlbumsHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Album>> HandleAsync(
        GetArtistAlbumsQuery query,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Albums
            .AsNoTracking()
            .Where(album => album.ArtistId == query.ArtistId)
            .OrderByDescending(album => album.ReleaseDate)
            .ThenBy(album => album.Title)
            .ToListAsync(cancellationToken);
    }
}