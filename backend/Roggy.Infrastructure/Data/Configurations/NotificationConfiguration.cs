using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roggy.Domain.Entities;

namespace Roggy.Infrastructure.Configurations;

public class NotificationConfiguration
    : IEntityTypeConfiguration<Notification>
{
    public void Configure(
        EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications");

        builder.HasKey(notification => notification.Id);

        builder.Property(notification => notification.UserId)
            .IsRequired();

        builder.Property(notification => notification.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(notification => notification.Message)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(notification => notification.ActionUrl)
            .HasMaxLength(500);

        builder.Property(notification => notification.IsRead)
            .IsRequired();

        builder.Property(notification => notification.ReadAt);

        builder.Property(notification => notification.CreatedAt)
            .IsRequired();

        builder.Property(notification => notification.UpdatedAt)
            .IsRequired();

        builder.HasIndex(notification => new
        {
            notification.UserId,
            notification.IsRead
        });

        builder.HasIndex(notification => new
        {
            notification.UserId,
            notification.CreatedAt
        });

        builder.HasOne(notification => notification.User)
            .WithMany()
            .HasForeignKey(notification => notification.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}