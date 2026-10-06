using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roggy.Domain.Entities;

namespace Roggy.Infrastructure.Configurations;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable("genres");

        builder.HasKey(genre => genre.Id);

        builder.Property(genre => genre.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(genre => genre.Name)
            .IsUnique();

        builder.Property(genre => genre.Slug)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(genre => genre.Slug)
            .IsUnique();

        builder.Property(genre => genre.CreatedAt)
            .IsRequired();

        builder.Property(genre => genre.UpdatedAt)
            .IsRequired();
    }
}