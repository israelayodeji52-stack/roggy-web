using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Infrastructure.Data;

public class RoggyDbContext : DbContext, IApplicationDbContext
{
    public RoggyDbContext(
        DbContextOptions<RoggyDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Album> Albums => Set<Album>();

    public DbSet<ArtistFollow> ArtistFollows => Set<ArtistFollow>();

    public DbSet<ArtistProfile> ArtistProfiles => Set<ArtistProfile>();

    public DbSet<ContentPreference> ContentPreferences => Set<ContentPreference>();

    public DbSet<DownloadHistory> DownloadHistories => Set<DownloadHistory>();

    public DbSet<Genre> Genres => Set<Genre>();

    public DbSet<ListeningHistory> ListeningHistories => Set<ListeningHistory>();

    public DbSet<LiveChatMessage> LiveChatMessages => Set<LiveChatMessage>();

    public DbSet<LiveEvent> LiveEvents => Set<LiveEvent>();

    public DbSet<LiveStream> LiveStreams => Set<LiveStream>();

    public DbSet<Notification> Notifications => Set<Notification>();

    public DbSet<Playlist> Playlists => Set<Playlist>();

    public DbSet<PlaylistItem> PlaylistItems => Set<PlaylistItem>();

    public DbSet<Podcast> Podcasts => Set<Podcast>();

    public DbSet<PodcastEpisode> PodcastEpisodes => Set<PodcastEpisode>();

    public DbSet<PodcastFollow> PodcastFollows => Set<PodcastFollow>();

    public DbSet<Remix> Remixes => Set<Remix>();

    public DbSet<Report> Reports => Set<Report>();

    public DbSet<SavedSong> SavedSongs => Set<SavedSong>();

    public DbSet<SongLike> SongLikes => Set<SongLike>();

    public DbSet<Song> Songs => Set<Song>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(RoggyDbContext).Assembly);
    }
}