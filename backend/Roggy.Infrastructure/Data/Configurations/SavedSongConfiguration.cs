using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roggy.Domain.Entities;

namespace Roggy.Infrastructure.Configurations;

public class SavedSongConfiguration : IEntityTypeConfiguration<SavedSong>
{
    public void Configure(EntityTypeBuilder<SavedSong> builder)
    {
        builder.ToTable("saved_songs");

        builder.HasKey(saved => saved.Id);

        builder.Property(saved => saved.UserId)
            .IsRequired();

        builder.Property(saved => saved.SongId)
            .IsRequired();

        builder.HasIndex(saved => new
        {
            saved.UserId,
            saved.SongId
        })
        .IsUnique();

        builder.HasOne(saved => saved.User)
            .WithMany()
            .HasForeignKey(saved => saved.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(saved => saved.Song)
            .WithMany()
            .HasForeignKey(saved => saved.SongId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}