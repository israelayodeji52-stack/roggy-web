using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roggy.Domain.Entities;

namespace Roggy.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Username)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(user => user.Username)
            .IsUnique();

        builder.Property(user => user.Email)
            .IsRequired()
            .HasMaxLength(320);

        builder.HasIndex(user => user.Email)
            .IsUnique();

        builder.Property(user => user.PasswordHash)
            .IsRequired();

        builder.Property(user => user.DisplayName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(user => user.ProfileImageUrl)
            .HasMaxLength(500);

        builder.Property(user => user.Bio)
            .HasMaxLength(1000);

        builder.Property(user => user.IsActive)
            .IsRequired();

        builder.Property(user => user.CreatedAt)
            .IsRequired();

        builder.Property(user => user.UpdatedAt)
            .IsRequired();

        builder.HasOne(user => user.ArtistProfile)
            .WithOne(profile => profile.User)
            .HasForeignKey<ArtistProfile>(profile => profile.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}