using CodeAtlas.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeAtlas.Api.Database.Configurations;

public sealed class SnippetTechnologyConfiguration : IEntityTypeConfiguration<SnippetTechnology>
{
    public void Configure(EntityTypeBuilder<SnippetTechnology> builder)
    {
        builder.HasKey(st => new { st.SnippetId, st.TechnologyId });
        
        builder.HasOne<Technology>()
            .WithMany()
            .HasForeignKey(st => st.TechnologyId);
        
        builder.HasOne<Snippet>()
            .WithMany(s => s.SnippetTechnologies)
            .HasForeignKey(st => st.SnippetId);
    }
}
