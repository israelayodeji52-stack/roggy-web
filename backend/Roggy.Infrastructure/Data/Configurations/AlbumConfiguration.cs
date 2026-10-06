using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roggy.Domain.Entities;

namespace Roggy.Infrastructure.Configurations;

public class AlbumConfiguration : IEntityTypeConfiguration<Album>
{
    public void Configure(EntityTypeBuilder<Album> builder)
    {
        builder.ToTable("albums");

        builder.HasKey(album => album.Id);

        builder.Property(album => album.ArtistId)
            .IsRequired();

        builder.Property(album => album.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(album => album.Description)
            .HasMaxLength(2000);

        builder.Property(album => album.CoverImageUrl)
            .HasMaxLength(500);

        builder.Property(album => album.ReleaseDate);

        builder.Property(album => album.IsPublished)
            .IsRequired();

        builder.Property(album => album.CreatedAt)
            .IsRequired();

        builder.Property(album => album.UpdatedAt)
            .IsRequired();

        builder.HasIndex(album => album.ArtistId);

        builder.HasOne(album => album.Artist)
            .WithMany()
            .HasForeignKey(album => album.ArtistId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(album => album.Songs)
            .WithOne(song => song.Album)
            .HasForeignKey(song => song.AlbumId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}