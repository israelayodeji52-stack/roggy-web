using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roggy.Domain.Entities;

namespace Roggy.Infrastructure.Configurations;

public class PlaylistItemConfiguration
    : IEntityTypeConfiguration<PlaylistItem>
{
    public void Configure(EntityTypeBuilder<PlaylistItem> builder)
    {
        builder.ToTable("playlist_items");

        builder.HasKey(item => item.Id);

        builder.Property(item => item.PlaylistId)
            .IsRequired();

        builder.Property(item => item.SongId)
            .IsRequired();

        builder.Property(item => item.Position)
            .IsRequired();

        builder.Property(item => item.AddedAt)
            .IsRequired();

        builder.Property(item => item.CreatedAt)
            .IsRequired();

        builder.Property(item => item.UpdatedAt)
            .IsRequired();

        builder.HasIndex(item => new
        {
            item.PlaylistId,
            item.Position
        })
        .IsUnique();

        builder.HasIndex(item => new
        {
            item.PlaylistId,
            item.SongId
        })
        .IsUnique();

        builder.HasOne(item => item.Playlist)
            .WithMany(playlist => playlist.Items)
            .HasForeignKey(item => item.PlaylistId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(item => item.Song)
            .WithMany()
            .HasForeignKey(item => item.SongId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}