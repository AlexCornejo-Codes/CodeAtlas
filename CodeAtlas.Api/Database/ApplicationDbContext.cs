using CodeAtlas.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodeAtlas.Api.Database;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Snippet> Snippets { get; set; }
    public DbSet<Technology> Technologies { get; set; }
    public DbSet<SnippetTechnology> SnippetTechnologies { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Application);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
