using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roggy.Domain.Entities;

namespace Roggy.Infrastructure.Configurations;

public class RemixConfiguration
    : IEntityTypeConfiguration<Remix>
{
    public void Configure(
        EntityTypeBuilder<Remix> builder)
    {
        builder.ToTable("remixes");

        builder.HasKey(remix => remix.Id);

        builder.Property(remix => remix.OriginalSongId)
            .IsRequired();

        builder.Property(remix => remix.RemixerArtistId)
            .IsRequired();

        builder.Property(remix => remix.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(remix => remix.AudioUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(remix => remix.CoverImageUrl)
            .HasMaxLength(500);

        builder.Property(remix => remix.Duration)
            .IsRequired();

        builder.Property(remix => remix.ContentRating)
            .IsRequired();

        builder.Property(remix => remix.IsPublished)
            .IsRequired();

        builder.Property(remix => remix.CreatedAt)
            .IsRequired();

        builder.Property(remix => remix.UpdatedAt)
            .IsRequired();

        builder.HasIndex(remix => remix.OriginalSongId);

        builder.HasIndex(remix => remix.RemixerArtistId);

        builder.HasIndex(remix => remix.IsPublished);

        builder.HasOne(remix => remix.OriginalSong)
            .WithMany(song => song.Remixes)
            .HasForeignKey(remix => remix.OriginalSongId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(remix => remix.RemixerArtist)
            .WithMany()
            .HasForeignKey(remix => remix.RemixerArtistId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}