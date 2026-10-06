using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roggy.Domain.Entities;

namespace Roggy.Infrastructure.Configurations;

public class ContentPreferenceConfiguration : IEntityTypeConfiguration<ContentPreference>
{
    public void Configure(EntityTypeBuilder<ContentPreference> builder)
    {
        builder.ToTable("content_preferences");

        builder.HasKey(preference => preference.Id);

        builder.Property(preference => preference.UserId)
            .IsRequired();

        builder.Property(preference => preference.AllowExplicitMusic)
            .IsRequired();

        builder.Property(preference => preference.AllowExplicitPodcasts)
            .IsRequired();

        builder.Property(preference => preference.AllowMatureContent)
            .IsRequired();

        builder.HasIndex(preference => preference.UserId)
            .IsUnique();

        builder.HasOne(preference => preference.User)
            .WithMany()
            .HasForeignKey(preference => preference.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}