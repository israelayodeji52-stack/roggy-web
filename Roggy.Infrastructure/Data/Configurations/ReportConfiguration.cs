using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roggy.Domain.Entities;

namespace Roggy.Infrastructure.Configurations;

public class ReportConfiguration
    : IEntityTypeConfiguration<Report>
{
    public void Configure(
        EntityTypeBuilder<Report> builder)
    {
        builder.ToTable("reports");

        builder.HasKey(report => report.Id);

        builder.Property(report => report.ReporterUserId)
            .IsRequired();

        builder.Property(report => report.TargetType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(report => report.TargetId)
            .IsRequired();

        builder.Property(report => report.Reason)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(report => report.Description)
            .HasMaxLength(1000);

        builder.Property(report => report.Status)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(report => report.ReviewedByUserId);

        builder.Property(report => report.ReviewedAt);

        builder.Property(report => report.CreatedAt)
            .IsRequired();

        builder.Property(report => report.UpdatedAt)
            .IsRequired();

        builder.HasIndex(report => report.ReporterUserId);

        builder.HasIndex(report => new
        {
            report.TargetType,
            report.TargetId
        });

        builder.HasIndex(report => report.Status);

        builder.HasOne(report => report.Reporter)
            .WithMany()
            .HasForeignKey(report => report.ReporterUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}