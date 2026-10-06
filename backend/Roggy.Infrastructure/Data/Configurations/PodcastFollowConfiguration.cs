using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roggy.Domain.Entities;

namespace Roggy.Infrastructure.Configurations;

public class PodcastFollowConfiguration : IEntityTypeConfiguration<PodcastFollow>
{
    public void Configure(EntityTypeBuilder<PodcastFollow> builder)
    {
        builder.ToTable("podcast_follows");

        builder.HasKey(follow => follow.Id);

        builder.Property(follow => follow.UserId)
            .IsRequired();

        builder.Property(follow => follow.PodcastId)
            .IsRequired();

        builder.HasIndex(follow => new
        {
            follow.UserId,
            follow.PodcastId
        })
        .IsUnique();

        builder.HasOne(follow => follow.User)
            .WithMany()
            .HasForeignKey(follow => follow.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(follow => follow.Podcast)
            .WithMany()
            .HasForeignKey(follow => follow.PodcastId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}