using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roggy.Domain.Entities;

namespace Roggy.Infrastructure.Configurations;

public class PodcastConfiguration : IEntityTypeConfiguration<Podcast>
{
    public void Configure(EntityTypeBuilder<Podcast> builder)
    {
        builder.ToTable("podcasts");

        builder.HasKey(podcast => podcast.Id);

        builder.Property(podcast => podcast.CreatorId)
            .IsRequired();

        builder.Property(podcast => podcast.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(podcast => podcast.Description)
            .HasMaxLength(3000);

        builder.Property(podcast => podcast.CoverImageUrl)
            .HasMaxLength(500);

        builder.Property(podcast => podcast.Category)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(podcast => podcast.ContentRating)
            .IsRequired();

        builder.Property(podcast => podcast.IsPublished)
            .IsRequired();

        builder.Property(podcast => podcast.CreatedAt)
            .IsRequired();

        builder.Property(podcast => podcast.UpdatedAt)
            .IsRequired();

        builder.HasIndex(podcast => podcast.Title);

        builder.HasIndex(podcast => podcast.CreatorId);

        builder.HasIndex(podcast => podcast.Category);

        builder.HasOne(podcast => podcast.Creator)
            .WithMany()
            .HasForeignKey(podcast => podcast.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(podcast => podcast.Episodes)
            .WithOne(episode => episode.Podcast)
            .HasForeignKey(episode => episode.PodcastId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}