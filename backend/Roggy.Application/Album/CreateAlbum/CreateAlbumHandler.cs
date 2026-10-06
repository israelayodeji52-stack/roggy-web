using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Albums.CreateAlbum;

public sealed class CreateAlbumHandler
{
    private readonly IApplicationDbContext _dbContext;

    public CreateAlbumHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Album> HandleAsync(
        CreateAlbumCommand command,
        CancellationToken cancellationToken = default)
    {
        var artistExists = await _dbContext.ArtistProfiles
            .AnyAsync(
                artist => artist.Id == command.ArtistId,
                cancellationToken);

        if (!artistExists)
        {
            throw new KeyNotFoundException(
                "Artist was not found.");
        }

        var album = new Album(
            command.ArtistId,
            command.Title);

        _dbContext.Albums.Add(album);

        await _dbContext.SaveChangesAsync(
            cancellationToken);

        return album;
    }
}