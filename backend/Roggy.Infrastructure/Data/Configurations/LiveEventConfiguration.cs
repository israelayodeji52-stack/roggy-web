using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roggy.Domain.Entities;

namespace Roggy.Infrastructure.Configurations;

public class LiveEventConfiguration : IEntityTypeConfiguration<LiveEvent>
{
    public void Configure(EntityTypeBuilder<LiveEvent> builder)
    {
        builder.ToTable("live_events");

        builder.HasKey(eventEntity => eventEntity.Id);

        builder.Property(eventEntity => eventEntity.ArtistId)
            .IsRequired();

        builder.Property(eventEntity => eventEntity.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(eventEntity => eventEntity.Description)
            .HasMaxLength(3000);

        builder.Property(eventEntity => eventEntity.CoverImageUrl)
            .HasMaxLength(500);

        builder.Property(eventEntity => eventEntity.ScheduledStart)
            .IsRequired();

        builder.Property(eventEntity => eventEntity.ScheduledEnd)
            .IsRequired();

        builder.Property(eventEntity => eventEntity.Status)
            .IsRequired();

        builder.Property(eventEntity => eventEntity.IsPublic)
            .IsRequired();

        builder.Property(eventEntity => eventEntity.ContentRating)
            .IsRequired();

        builder.Property(eventEntity => eventEntity.CreatedAt)
            .IsRequired();

        builder.Property(eventEntity => eventEntity.UpdatedAt)
            .IsRequired();

        builder.HasIndex(eventEntity => eventEntity.ArtistId);

        builder.HasIndex(eventEntity => eventEntity.ScheduledStart);

        builder.HasIndex(eventEntity => eventEntity.Status);

        builder.HasOne(eventEntity => eventEntity.Artist)
            .WithMany()
            .HasForeignKey(eventEntity => eventEntity.ArtistId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(eventEntity => eventEntity.LiveStream)
            .WithOne(stream => stream.LiveEvent)
            .HasForeignKey<LiveStream>(stream => stream.LiveEventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}