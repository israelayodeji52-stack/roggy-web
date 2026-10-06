using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roggy.Domain.Entities;

namespace Roggy.Infrastructure.Configurations;

public class ArtistFollowConfiguration
    : IEntityTypeConfiguration<ArtistFollow>
{
    public void Configure(
        EntityTypeBuilder<ArtistFollow> builder)
    {
        builder.ToTable("artist_follows");

        builder.HasKey(follow => follow.Id);

        builder.Property(follow => follow.UserId)
            .IsRequired();

        builder.Property(follow => follow.ArtistId)
            .IsRequired();

        builder.Property(follow => follow.CreatedAt)
            .IsRequired();

        builder.Property(follow => follow.UpdatedAt)
            .IsRequired();

        builder.HasIndex(follow => new
        {
            follow.UserId,
            follow.ArtistId
        })
        .IsUnique();

        builder.HasIndex(follow => follow.ArtistId);

        builder.HasOne(follow => follow.User)
            .WithMany()
            .HasForeignKey(follow => follow.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(follow => follow.Artist)
            .WithMany()
            .HasForeignKey(follow => follow.ArtistId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}