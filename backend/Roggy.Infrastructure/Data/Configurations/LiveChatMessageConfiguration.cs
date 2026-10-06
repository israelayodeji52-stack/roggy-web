using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roggy.Domain.Entities;

namespace Roggy.Infrastructure.Configurations;

public class LiveChatMessageConfiguration : IEntityTypeConfiguration<LiveChatMessage>
{
    public void Configure(EntityTypeBuilder<LiveChatMessage> builder)
    {
        builder.ToTable("live_chat_messages");

        builder.HasKey(message => message.Id);

        builder.Property(message => message.LiveEventId)
            .IsRequired();

        builder.Property(message => message.UserId)
            .IsRequired();

        builder.Property(message => message.Message)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(message => message.IsDeleted)
            .IsRequired();

        builder.Property(message => message.CreatedAt)
            .IsRequired();

        builder.Property(message => message.UpdatedAt)
            .IsRequired();

        builder.HasIndex(message => message.LiveEventId);

        builder.HasIndex(message => message.UserId);

        builder.HasIndex(message => message.CreatedAt);

        builder.HasOne(message => message.LiveEvent)
            .WithMany(eventEntity => eventEntity.ChatMessages)
            .HasForeignKey(message => message.LiveEventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(message => message.User)
            .WithMany()
            .HasForeignKey(message => message.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}