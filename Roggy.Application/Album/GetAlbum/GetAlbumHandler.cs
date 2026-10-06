using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Albums.GetAlbum;

public sealed class GetAlbumHandler
{
    private readonly IApplicationDbContext _dbContext;

    public GetAlbumHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Album?> HandleAsync(
        GetAlbumQuery query,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Albums
            .AsNoTracking()
            .Include(album => album.Artist)
            .Include(album => album.Songs)
            .FirstOrDefaultAsync(
                album => album.Id == query.AlbumId,
                cancellationToken);
    }
}