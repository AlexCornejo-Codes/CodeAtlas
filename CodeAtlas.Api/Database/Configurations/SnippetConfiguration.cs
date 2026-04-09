using CodeAtlas.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeAtlas.Api.Database.Configurations;

public sealed class SnippetConfiguration : IEntityTypeConfiguration<Snippet>
{
    public void Configure(EntityTypeBuilder<Snippet> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(s => s.Title)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.Description)
            .HasMaxLength(500);

        builder.Property(s => s.Code)
            .HasColumnType("text")
            .IsRequired();
        
        builder.HasMany(s => s.Technologies)
            .WithMany()
            .UsingEntity<SnippetTechnology>();
        
        builder.HasIndex(s => s.CreatedAtUtc);
        builder.HasIndex(s => s.Language);
        builder.HasIndex(s => s.Visibility);
        builder.HasIndex(s => s.IsArchived);
    }
}
