using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roggy.Domain.Entities;

namespace Roggy.Infrastructure.Configurations;

public class LiveStreamConfiguration : IEntityTypeConfiguration<LiveStream>
{
    public void Configure(EntityTypeBuilder<LiveStream> builder)
    {
        builder.ToTable("live_streams");

        builder.HasKey(stream => stream.Id);

        builder.Property(stream => stream.LiveEventId)
            .IsRequired();

        builder.Property(stream => stream.PlaybackUrl)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(stream => stream.StreamStatus)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(stream => stream.StartedAt);

        builder.Property(stream => stream.EndedAt);

        builder.Property(stream => stream.CreatedAt)
            .IsRequired();

        builder.Property(stream => stream.UpdatedAt)
            .IsRequired();

        builder.HasIndex(stream => stream.LiveEventId)
            .IsUnique();

        builder.HasOne(stream => stream.LiveEvent)
            .WithOne(eventEntity => eventEntity.LiveStream)
            .HasForeignKey<LiveStream>(stream => stream.LiveEventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}