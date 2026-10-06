using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roggy.Domain.Entities;

namespace Roggy.Infrastructure.Configurations;

public class DownloadHistoryConfiguration
    : IEntityTypeConfiguration<DownloadHistory>
{
    public void Configure(
        EntityTypeBuilder<DownloadHistory> builder)
    {
        builder.ToTable("download_histories");

        builder.HasKey(history => history.Id);

        builder.Property(history => history.UserId)
            .IsRequired();

        builder.Property(history => history.SongId)
            .IsRequired();

        builder.Property(history => history.DownloadedAt)
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
            history.DownloadedAt
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