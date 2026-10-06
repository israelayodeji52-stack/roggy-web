using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roggy.Domain.Entities;

namespace Roggy.Infrastructure.Configurations;

public class SongConfiguration : IEntityTypeConfiguration<Song>
{
    public void Configure(EntityTypeBuilder<Song> builder)
    {
        builder.ToTable("songs");

        builder.HasKey(song => song.Id);

        builder.Property(song => song.ArtistId)
            .IsRequired();

        builder.Property(song => song.AlbumId);

        builder.Property(song => song.GenreId)
            .IsRequired();

        builder.Property(song => song.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(song => song.Description)
            .HasMaxLength(2000);

        builder.Property(song => song.AudioUrl)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(song => song.CoverImageUrl)
            .HasMaxLength(500);

        builder.Property(song => song.Duration)
            .IsRequired();

        builder.Property(song => song.ReleaseDate);

        builder.Property(song => song.ContentRating)
            .IsRequired();

        builder.Property(song => song.AllowRemixing)
            .IsRequired();

        builder.Property(song => song.IsPublished)
            .IsRequired();

        builder.Property(song => song.PlayCount)
            .IsRequired();

        builder.Property(song => song.TrackNumber)
            .IsRequired();

        builder.Property(song => song.CreatedAt)
            .IsRequired();

        builder.Property(song => song.UpdatedAt)
            .IsRequired();

        builder.HasIndex(song => song.Title);

        builder.HasIndex(song => song.ArtistId);

        builder.HasIndex(song => song.GenreId);

        builder.HasIndex(song => song.AlbumId);

        builder.HasOne(song => song.Artist)
            .WithMany()
            .HasForeignKey(song => song.ArtistId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(song => song.Genre)
            .WithMany()
            .HasForeignKey(song => song.GenreId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(song => song.Album)
            .WithMany(album => album.Songs)
            .HasForeignKey(song => song.AlbumId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}