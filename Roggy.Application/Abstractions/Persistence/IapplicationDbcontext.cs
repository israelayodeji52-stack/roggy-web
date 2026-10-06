using Microsoft.EntityFrameworkCore;
using Roggy.Domain.Entities;
using ListeningHistoryEntity = Roggy.Domain.Entities.ListeningHistory;
using PlaylistEntity = Roggy.Domain.Entities.Playlist;

namespace Roggy.Application.Abstractions.Persistence;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }

    DbSet<ArtistProfile> ArtistProfiles { get; }

    DbSet<ArtistFollow> ArtistFollows { get; }

    DbSet<ContentPreference> ContentPreferences { get; }

    DbSet<Genre> Genres { get; }

    DbSet<Song> Songs { get; }

    DbSet<Album> Albums { get; }

    DbSet<SongLike> SongLikes { get; }

    DbSet<SavedSong> SavedSongs { get; }

    DbSet<ListeningHistoryEntity> ListeningHistories { get; }

    DbSet<DownloadHistory> DownloadHistories { get; }

    DbSet<PlaylistEntity> Playlists { get; }

    DbSet<PlaylistItem> PlaylistItems { get; }

    DbSet<Remix> Remixes { get; }

    DbSet<Notification> Notifications { get; }

    DbSet<Report> Reports { get; }

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}