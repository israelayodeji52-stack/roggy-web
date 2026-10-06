using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roggy.Domain.Entities;

namespace Roggy.Infrastructure.Configurations;

public class ListeningHistoryConfiguration
    : IEntityTypeConfiguration<ListeningHistory>
{
    public void Configure(
        EntityTypeBuilder<ListeningHistory> builder)
    {
        builder.ToTable("listening_histories");

        builder.HasKey(history => history.Id);

        builder.Property(history => history.UserId)
            .IsRequired();

        builder.Property(history => history.SongId)
            .IsRequired();

        builder.Property(history => history.StartedAt)
            .IsRequired();

        builder.Property(history => history.CompletedAt)
            .IsRequired(false);

        builder.Property(history => history.DurationPlayed)
            .IsRequired();

        builder.Property(history => history.CreatedAt)
            .IsRequired();

        builder.Property(history => history.UpdatedAt)
            .IsRequired();

        builder.HasIndex(history => history.UserId);

        builder.HasIndex(history => history.SongId);

        builder.HasIndex(history => new
        {
            history.UserId,
            history.StartedAt
        });

        builder.HasOne(history => history.User)
            .WithMany()
            .HasForeignKey(history => history.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(history => history.Song)
            .WithMany()
            .HasForeignKey(history => history.SongId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}