using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using PlaylistEntity = Roggy.Domain.Entities.Playlist;

namespace Roggy.Application.Playlist.MakePlaylistPrivate;

public sealed class MakePlaylistPrivateHandler
{
    private readonly IApplicationDbContext _dbContext;

    public MakePlaylistPrivateHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PlaylistEntity> HandleAsync(
        MakePlaylistPrivateCommand command,
        CancellationToken cancellationToken = default)
    {
        var playlist = await _dbContext.Playlists
            .FirstOrDefaultAsync(
                playlist =>
                    playlist.Id == command.PlaylistId &&
                    playlist.UserId == command.UserId,
                cancellationToken);

        if (playlist is null)
        {
            throw new KeyNotFoundException(
                "Playlist was not found.");
        }

        playlist.MakePrivate();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return playlist;
    }
}