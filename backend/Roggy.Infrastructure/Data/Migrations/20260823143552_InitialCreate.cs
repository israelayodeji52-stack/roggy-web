using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roggy.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "genres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ProfileImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Bio = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "artist_profiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    StageName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Biography = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ProfileImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    BannerImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_artist_profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_artist_profiles_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "content_preferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AllowExplicitMusic = table.Column<bool>(type: "boolean", nullable: false),
                    AllowExplicitPodcasts = table.Column<bool>(type: "boolean", nullable: false),
                    AllowMatureContent = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_content_preferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_content_preferences_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    ActionUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_notifications_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "playlists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CoverImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_playlists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_playlists_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "podcasts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: true),
                    CoverImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ContentRating = table.Column<int>(type: "integer", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_podcasts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_podcasts_users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReporterUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TargetId = table.Column<Guid>(type: "uuid", nullable: false),
                    Reason = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ReviewedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_reports_users_ReporterUserId",
                        column: x => x.ReporterUserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "albums",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ArtistId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CoverImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_albums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_albums_artist_profiles_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "artist_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "artist_follows",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ArtistId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_artist_follows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_artist_follows_artist_profiles_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "artist_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_artist_follows_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "live_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ArtistId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: true),
                    CoverImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ScheduledStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ScheduledEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    ContentRating = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_live_events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_live_events_artist_profiles_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "artist_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "podcast_follows",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PodcastId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_podcast_follows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_podcast_follows_podcasts_PodcastId",
                        column: x => x.PodcastId,
                        principalTable: "podcasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_podcast_follows_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PodcastEpisodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PodcastId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    AudioUrl = table.Column<string>(type: "text", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    EpisodeNumber = table.Column<int>(type: "integer", nullable: false),
                    SeasonNumber = table.Column<int>(type: "integer", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ContentRating = table.Column<int>(type: "integer", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    PlayCount = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PodcastEpisodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PodcastEpisodes_podcasts_PodcastId",
                        column: x => x.PodcastId,
                        principalTable: "podcasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "songs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ArtistId = table.Column<Guid>(type: "uuid", nullable: false),
                    AlbumId = table.Column<Guid>(type: "uuid", nullable: true),
                    GenreId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    AudioUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CoverImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ContentRating = table.Column<int>(type: "integer", nullable: false),
                    AllowRemixing = table.Column<bool>(type: "boolean", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    PlayCount = table.Column<long>(type: "bigint", nullable: false),
                    TrackNumber = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_songs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_songs_albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_songs_artist_profiles_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "artist_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_songs_genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "live_chat_messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LiveEventId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Message = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_live_chat_messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_live_chat_messages_live_events_LiveEventId",
                        column: x => x.LiveEventId,
                        principalTable: "live_events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_live_chat_messages_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "live_streams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LiveEventId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlaybackUrl = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    StreamStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_live_streams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_live_streams_live_events_LiveEventId",
                        column: x => x.LiveEventId,
                        principalTable: "live_events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "download_history",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SongId = table.Column<Guid>(type: "uuid", nullable: false),
                    DownloadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_download_history", x => x.Id);
                    table.ForeignKey(
                        name: "FK_download_history_songs_SongId",
                        column: x => x.SongId,
                        principalTable: "songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_download_history_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "listening_history",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SongId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DurationPlayed = table.Column<TimeSpan>(type: "interval", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_listening_history", x => x.Id);
                    table.ForeignKey(
                        name: "FK_listening_history_songs_SongId",
                        column: x => x.SongId,
                        principalTable: "songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_listening_history_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "playlist_items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlaylistId = table.Column<Guid>(type: "uuid", nullable: false),
                    SongId = table.Column<Guid>(type: "uuid", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_playlist_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_playlist_items_playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_playlist_items_songs_SongId",
                        column: x => x.SongId,
                        principalTable: "songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "remixes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginalSongId = table.Column<Guid>(type: "uuid", nullable: false),
                    RemixerArtistId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AudioUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CoverImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    ContentRating = table.Column<int>(type: "integer", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_remixes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_remixes_artist_profiles_RemixerArtistId",
                        column: x => x.RemixerArtistId,
                        principalTable: "artist_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_remixes_songs_OriginalSongId",
                        column: x => x.OriginalSongId,
                        principalTable: "songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "saved_songs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SongId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_saved_songs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_saved_songs_songs_SongId",
                        column: x => x.SongId,
                        principalTable: "songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_saved_songs_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "song_likes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SongId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_song_likes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_song_likes_songs_SongId",
                        column: x => x.SongId,
                        principalTable: "songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_song_likes_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_albums_ArtistId",
                table: "albums",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_artist_follows_ArtistId",
                table: "artist_follows",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_artist_follows_UserId_ArtistId",
                table: "artist_follows",
                columns: new[] { "UserId", "ArtistId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_artist_profiles_StageName",
                table: "artist_profiles",
                column: "StageName");

            migrationBuilder.CreateIndex(
                name: "IX_artist_profiles_UserId",
                table: "artist_profiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_content_preferences_UserId",
                table: "content_preferences",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_download_history_DownloadedAt",
                table: "download_history",
                column: "DownloadedAt");

            migrationBuilder.CreateIndex(
                name: "IX_download_history_SongId",
                table: "download_history",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_download_history_UserId",
                table: "download_history",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_genres_Name",
                table: "genres",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_genres_Slug",
                table: "genres",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_listening_history_SongId",
                table: "listening_history",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_listening_history_StartedAt",
                table: "listening_history",
                column: "StartedAt");

            migrationBuilder.CreateIndex(
                name: "IX_listening_history_UserId",
                table: "listening_history",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_live_chat_messages_CreatedAt",
                table: "live_chat_messages",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_live_chat_messages_LiveEventId",
                table: "live_chat_messages",
                column: "LiveEventId");

            migrationBuilder.CreateIndex(
                name: "IX_live_chat_messages_UserId",
                table: "live_chat_messages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_live_events_ArtistId",
                table: "live_events",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_live_events_ScheduledStart",
                table: "live_events",
                column: "ScheduledStart");

            migrationBuilder.CreateIndex(
                name: "IX_live_events_Status",
                table: "live_events",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_live_streams_LiveEventId",
                table: "live_streams",
                column: "LiveEventId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_notifications_CreatedAt",
                table: "notifications",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_UserId",
                table: "notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_UserId_IsRead",
                table: "notifications",
                columns: new[] { "UserId", "IsRead" });

            migrationBuilder.CreateIndex(
                name: "IX_playlist_items_PlaylistId_Position",
                table: "playlist_items",
                columns: new[] { "PlaylistId", "Position" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_playlist_items_PlaylistId_SongId",
                table: "playlist_items",
                columns: new[] { "PlaylistId", "SongId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_playlist_items_SongId",
                table: "playlist_items",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_playlists_UserId",
                table: "playlists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_podcast_follows_PodcastId",
                table: "podcast_follows",
                column: "PodcastId");

            migrationBuilder.CreateIndex(
                name: "IX_podcast_follows_UserId_PodcastId",
                table: "podcast_follows",
                columns: new[] { "UserId", "PodcastId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PodcastEpisodes_PodcastId",
                table: "PodcastEpisodes",
                column: "PodcastId");

            migrationBuilder.CreateIndex(
                name: "IX_podcasts_Category",
                table: "podcasts",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_podcasts_CreatorId",
                table: "podcasts",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_podcasts_Title",
                table: "podcasts",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_remixes_OriginalSongId",
                table: "remixes",
                column: "OriginalSongId");

            migrationBuilder.CreateIndex(
                name: "IX_remixes_RemixerArtistId",
                table: "remixes",
                column: "RemixerArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_reports_ReporterUserId",
                table: "reports",
                column: "ReporterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_reports_Status",
                table: "reports",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_reports_TargetType_TargetId",
                table: "reports",
                columns: new[] { "TargetType", "TargetId" });

            migrationBuilder.CreateIndex(
                name: "IX_saved_songs_SongId",
                table: "saved_songs",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_saved_songs_UserId_SongId",
                table: "saved_songs",
                columns: new[] { "UserId", "SongId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_song_likes_SongId",
                table: "song_likes",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_song_likes_UserId_SongId",
                table: "song_likes",
                columns: new[] { "UserId", "SongId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_songs_AlbumId",
                table: "songs",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_songs_ArtistId",
                table: "songs",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_songs_GenreId",
                table: "songs",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_songs_Title",
                table: "songs",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                table: "users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_Username",
                table: "users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "artist_follows");

            migrationBuilder.DropTable(
                name: "content_preferences");

            migrationBuilder.DropTable(
                name: "download_history");

            migrationBuilder.DropTable(
                name: "listening_history");

            migrationBuilder.DropTable(
                name: "live_chat_messages");

            migrationBuilder.DropTable(
                name: "live_streams");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "playlist_items");

            migrationBuilder.DropTable(
                name: "podcast_follows");

            migrationBuilder.DropTable(
                name: "PodcastEpisodes");

            migrationBuilder.DropTable(
                name: "remixes");

            migrationBuilder.DropTable(
                name: "reports");

            migrationBuilder.DropTable(
                name: "saved_songs");

            migrationBuilder.DropTable(
                name: "song_likes");

            migrationBuilder.DropTable(
                name: "live_events");

            migrationBuilder.DropTable(
                name: "playlists");

            migrationBuilder.DropTable(
                name: "podcasts");

            migrationBuilder.DropTable(
                name: "songs");

            migrationBuilder.DropTable(
                name: "albums");

            migrationBuilder.DropTable(
                name: "genres");

            migrationBuilder.DropTable(
                name: "artist_profiles");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
