using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roggy.Domain.Entities;

namespace Roggy.Infrastructure.Configurations;

public class SongLikeConfiguration : IEntityTypeConfiguration<SongLike>
{
    public void Configure(EntityTypeBuilder<SongLike> builder)
    {
        builder.ToTable("song_likes");

        builder.HasKey(like => like.Id);

        builder.Property(like => like.UserId)
            .IsRequired();

        builder.Property(like => like.SongId)
            .IsRequired();

        builder.HasIndex(like => new
        {
            like.UserId,
            like.SongId
        })
        .IsUnique();

        builder.HasOne(like => like.User)
            .WithMany()
            .HasForeignKey(like => like.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(like => like.Song)
            .WithMany()
            .HasForeignKey(like => like.SongId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}