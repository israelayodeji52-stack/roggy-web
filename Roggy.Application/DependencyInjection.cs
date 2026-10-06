using Microsoft.Extensions.DependencyInjection;
using Roggy.Application.Albums.CreateAlbum;
using Roggy.Application.Albums.DeleteAlbum;
using Roggy.Application.Albums.GetAlbum;
using Roggy.Application.Albums.GetArtistAlbums;
using Roggy.Application.Albums.PublishAlbum;
using Roggy.Application.Albums.UnpublishAlbum;
using Roggy.Application.Albums.UpdateAlbum;
using Roggy.Application.Artists.CreateArtistProfile;
using Roggy.Application.Artists.FollowArtist;
using Roggy.Application.Artists.GetArtistFollowStatus;
using Roggy.Application.Artists.GetArtistFollowerCount;
using Roggy.Application.Artists.GetArtistProfile;
using Roggy.Application.Artists.GetArtists;
using Roggy.Application.Artists.GetFollowedArtists;
using Roggy.Application.Artists.GetPopularArtists;
using Roggy.Application.Artists.SearchArtists;
using Roggy.Application.Artists.UnfollowArtist;
using Roggy.Application.Artists.UnverifyArtist;
using Roggy.Application.Artists.UpdateArtistProfile;
using Roggy.Application.Artists.VerifyArtist;
using Roggy.Application.ContentPreferences.GetContentPreference;
using Roggy.Application.ContentPreferences.UpdateContentPreference;
using Roggy.Application.Downloads.DeleteDownloadHistoryEntry;
using Roggy.Application.Downloads.DownloadSong;
using Roggy.Application.Downloads.GetDownloadHistory;
using Roggy.Application.Downloads.GetDownloadHistoryEntry;
using Roggy.Application.Genres.CreateGenre;
using Roggy.Application.ListeningHistory.DeleteListeningHistoryEntry;
using Roggy.Application.ListeningHistory.GetListeningHistory;
using Roggy.Application.ListeningHistory.GetListeningHistoryEntry;
using Roggy.Application.ListeningHistory.RecordListeningHistory;
using Roggy.Application.Notifications.CreateNotification;
using Roggy.Application.Notifications.GetNotifications;
using Roggy.Application.Notifications.MarkNotificationAsRead;
using Roggy.Application.Playlist.AddSongToPlaylist;
using Roggy.Application.Playlist.CreatePlaylist;
using Roggy.Application.Playlist.DeletePlaylist;
using Roggy.Application.Playlist.GetPlaylist;
using Roggy.Application.Playlist.GetPublicPlaylists;
using Roggy.Application.Playlist.GetUserPlaylists;
using Roggy.Application.Playlist.MakePlaylistPrivate;
using Roggy.Application.Playlist.MakePlaylistPublic;
using Roggy.Application.Playlist.RemoveSongFromPlaylist;
using Roggy.Application.Playlist.ReorderPlaylistSong;
using Roggy.Application.Playlist.UpdatePlaylist;
using Roggy.Application.Remixes.CreateRemix;
using Roggy.Application.Remixes.DeleteRemix;
using Roggy.Application.Remixes.GetRemix;
using Roggy.Application.Remixes.GetSongRemixes;
using Roggy.Application.Remixes.PublishRemix;
using Roggy.Application.Remixes.UnpublishRemix;
using Roggy.Application.Remixes.UpdateRemix;
using Roggy.Application.Reports.ApproveReport;
using Roggy.Application.Reports.CreateReport;
using Roggy.Application.Reports.GetReports;
using Roggy.Application.Reports.RejectReport;
using Roggy.Application.Songs.CreateSong;
using Roggy.Application.Songs.DeleteSong;
using Roggy.Application.Songs.GetLikedSongs;
using Roggy.Application.Songs.GetRecentlyReleasedSongs;
using Roggy.Application.Songs.GetRecommendedSongs;
using Roggy.Application.Songs.GetSavedSongs;
using Roggy.Application.Songs.GetSavedSongStatus;
using Roggy.Application.Songs.GetSong;
using Roggy.Application.Songs.GetSongLikes;
using Roggy.Application.Songs.GetTrendingSongs;
using Roggy.Application.Songs.IncrementPlayCount;
using Roggy.Application.Songs.LikeSong;
using Roggy.Application.Songs.PublishSong;
using Roggy.Application.Songs.SaveSong;
using Roggy.Application.Songs.SavedSong;
using Roggy.Application.Songs.SearchSongs;
using Roggy.Application.Songs.UnlikeSong;
using Roggy.Application.Songs.UnpublishSong;
using Roggy.Application.Songs.UnsaveSong;
using Roggy.Application.Songs.UpdateSong;
using Roggy.Application.Users.ActivateUser;
using Roggy.Application.Users.CreateUser;
using Roggy.Application.Users.DeactivateUser;
using Roggy.Application.Users.Login;

namespace Roggy.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<CreateUserHandler>();
        services.AddScoped<LoginHandler>();
        services.AddScoped<ActivateUserHandler>();
        services.AddScoped<DeactivateUserHandler>();

        services.AddScoped<CreateArtistProfileHandler>();
        services.AddScoped<GetArtistProfileHandler>();
        services.AddScoped<GetArtistsHandler>();
        services.AddScoped<GetPopularArtistsHandler>();
        services.AddScoped<SearchArtistsHandler>();
        services.AddScoped<UpdateArtistProfileHandler>();
        services.AddScoped<VerifyArtistHandler>();
        services.AddScoped<UnverifyArtistHandler>();

        services.AddScoped<FollowArtistHandler>();
        services.AddScoped<UnfollowArtistHandler>();
        services.AddScoped<GetArtistFollowStatusHandler>();
        services.AddScoped<GetFollowedArtistsHandler>();
        services.AddScoped<GetArtistFollowerCountHandler>();

        services.AddScoped<CreateGenreHandler>();

        services.AddScoped<CreateAlbumHandler>();
        services.AddScoped<GetAlbumHandler>();
        services.AddScoped<GetArtistAlbumsHandler>();
        services.AddScoped<UpdateAlbumHandler>();
        services.AddScoped<DeleteAlbumHandler>();
        services.AddScoped<PublishAlbumHandler>();
        services.AddScoped<UnpublishAlbumHandler>();

        services.AddScoped<CreateSongHandler>();
        services.AddScoped<GetSongHandler>();
        services.AddScoped<GetLikedSongsHandler>();
        services.AddScoped<GetSavedSongsHandler>();
        services.AddScoped<GetSavedSongStatusHandler>();
        services.AddScoped<UpdateSongHandler>();
        services.AddScoped<DeleteSongHandler>();
        services.AddScoped<PublishSongHandler>();
        services.AddScoped<UnpublishSongHandler>();
        services.AddScoped<IncrementPlayCountHandler>();
        services.AddScoped<LikeSongHandler>();
        services.AddScoped<UnlikeSongHandler>();
        services.AddScoped<GetSongLikesHandler>();
        services.AddScoped<SavedSongHandler>();
        services.AddScoped<UnsaveSongHandler>();
        services.AddScoped<SearchSongsHandler>();
        services.AddScoped<GetRecentlyReleasedSongsHandler>();
        services.AddScoped<GetTrendingSongsHandler>();
        services.AddScoped<GetRecommendedSongsHandler>();

        services.AddScoped<CreatePlaylistHandler>();
        services.AddScoped<GetUserPlaylistsHandler>();
        services.AddScoped<GetPlaylistHandler>();
        services.AddScoped<UpdatePlaylistHandler>();
        services.AddScoped<DeletePlaylistHandler>();
        services.AddScoped<AddSongToPlaylistHandler>();
        services.AddScoped<RemoveSongFromPlaylistHandler>();
        services.AddScoped<ReorderPlaylistSongHandler>();
        services.AddScoped<MakePlaylistPublicHandler>();
        services.AddScoped<MakePlaylistPrivateHandler>();
        services.AddScoped<GetPublicPlaylistsHandler>();

        services.AddScoped<RecordListeningHistoryHandler>();
        services.AddScoped<GetListeningHistoryHandler>();
        services.AddScoped<GetListeningHistoryEntryHandler>();
        services.AddScoped<DeleteListeningHistoryEntryHandler>();

        services.AddScoped<DownloadSongHandler>();
        services.AddScoped<GetDownloadHistoryHandler>();
        services.AddScoped<GetDownloadHistoryEntryHandler>();
        services.AddScoped<DeleteDownloadHistoryEntryHandler>();

        services.AddScoped<CreateRemixHandler>();
        services.AddScoped<GetRemixHandler>();
        services.AddScoped<GetSongRemixesHandler>();
        services.AddScoped<UpdateRemixHandler>();
        services.AddScoped<DeleteRemixHandler>();
        services.AddScoped<PublishRemixHandler>();
        services.AddScoped<UnpublishRemixHandler>();

        services.AddScoped<GetContentPreferenceHandler>();
        services.AddScoped<UpdateContentPreferenceHandler>();

        services.AddScoped<CreateNotificationHandler>();
        services.AddScoped<GetNotificationsHandler>();
        services.AddScoped<MarkNotificationAsReadHandler>();

        services.AddScoped<CreateReportHandler>();
        services.AddScoped<ApproveReportHandler>();
        services.AddScoped<RejectReportHandler>();
        services.AddScoped<GetReportsHandler>();

        return services;
    }
}