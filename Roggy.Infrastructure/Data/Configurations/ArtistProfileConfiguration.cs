using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roggy.Domain.Entities;

namespace Roggy.Infrastructure.Configurations;

public class ArtistProfileConfiguration : IEntityTypeConfiguration<ArtistProfile>
{
    public void Configure(EntityTypeBuilder<ArtistProfile> builder)
    {
        builder.ToTable("artist_profiles");

        builder.HasKey(artist => artist.Id);

        builder.Property(artist => artist.UserId)
            .IsRequired();

        builder.HasIndex(artist => artist.UserId)
            .IsUnique();

        builder.Property(artist => artist.StageName)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(artist => artist.StageName);

        builder.Property(artist => artist.Biography)
            .HasMaxLength(2000);

        builder.Property(artist => artist.ProfileImageUrl)
            .HasMaxLength(500);

        builder.Property(artist => artist.BannerImageUrl)
            .HasMaxLength(500);

        builder.Property(artist => artist.IsVerified)
            .IsRequired();

        builder.Property(artist => artist.CreatedAt)
            .IsRequired();

        builder.Property(artist => artist.UpdatedAt)
            .IsRequired();

        builder.HasOne(artist => artist.User)
            .WithOne(user => user.ArtistProfile)
            .HasForeignKey<ArtistProfile>(artist => artist.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}