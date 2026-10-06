using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Albums.PublishAlbum;

public sealed class PublishAlbumHandler
{
    private readonly IApplicationDbContext _dbContext;

    public PublishAlbumHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Album> HandleAsync(
        PublishAlbumCommand command,
        CancellationToken cancellationToken = default)
    {
        var album = await _dbContext.Albums
            .FirstOrDefaultAsync(
                album => album.Id == command.AlbumId,
                cancellationToken);

        if (album is null)
        {
            throw new KeyNotFoundException(
                "Album was not found.");
        }

        album.Publish();

        await _dbContext.SaveChangesAsync(
            cancellationToken);

        return album;
    }
}