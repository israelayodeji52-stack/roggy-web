using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Songs.CreateSong;

public sealed class CreateSongHandler
{
    private readonly IApplicationDbContext _dbContext;

    public CreateSongHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Song> HandleAsync(
        CreateSongCommand command,
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

        if (command.AlbumId.HasValue)
        {
            var albumExists = await _dbContext.Albums
                .AnyAsync(
                    album =>
                        album.Id == command.AlbumId.Value &&
                        album.ArtistId == command.ArtistId,
                    cancellationToken);

            if (!albumExists)
            {
                throw new KeyNotFoundException(
                    "Album was not found for this artist.");
            }
        }

        var song = new Song(
            command.ArtistId,
            command.GenreId,
            command.Title,
            command.AudioUrl,
            command.Duration);

        song.SetAlbum(command.AlbumId);

        song.UpdateSong(
            command.GenreId,
            command.Title,
            command.Description,
            command.AudioUrl,
            command.CoverImageUrl,
            command.Duration,
            command.ReleaseDate,
            command.ContentRating,
            command.AllowRemixing,
            command.TrackNumber);

        _dbContext.Songs.Add(song);

        await _dbContext.SaveChangesAsync(
            cancellationToken);

        return song;
    }
}