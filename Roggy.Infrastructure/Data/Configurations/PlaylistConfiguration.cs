using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roggy.Domain.Entities;

namespace Roggy.Infrastructure.Configurations;

public class PlaylistConfiguration : IEntityTypeConfiguration<Playlist>
{
    public void Configure(EntityTypeBuilder<Playlist> builder)
    {
        builder.ToTable("playlists");

        builder.HasKey(playlist => playlist.Id);

        builder.Property(playlist => playlist.UserId)
            .IsRequired();

        builder.Property(playlist => playlist.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(playlist => playlist.Description)
            .HasMaxLength(1000);

        builder.Property(playlist => playlist.CoverImageUrl)
            .HasMaxLength(500);

        builder.Property(playlist => playlist.IsPublic)
            .IsRequired();

        builder.Property(playlist => playlist.CreatedAt)
            .IsRequired();

        builder.Property(playlist => playlist.UpdatedAt)
            .IsRequired();

        builder.HasIndex(playlist => playlist.UserId);

        builder.HasOne(playlist => playlist.User)
            .WithMany()
            .HasForeignKey(playlist => playlist.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(playlist => playlist.Items)
            .WithOne(item => item.Playlist)
            .HasForeignKey(item => item.PlaylistId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}