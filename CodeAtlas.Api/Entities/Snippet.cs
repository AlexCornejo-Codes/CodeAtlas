namespace CodeAtlas.Api.Entities;

public sealed class Snippet
{
    public string Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Code { get; set; } = string.Empty;
    public Language Language { get; set; }
    public Visibility Visibility { get; set; }
    public bool IsArchived { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }

    public List<SnippetTechnology> SnippetTechnologies { get; set; }
    public List<Technology> Technologies { get; set; }
}

public enum Language
{
    CSharp = 0,
    Sql = 1,
    Python = 2
}

public enum Visibility
{
    Public = 0,
    Private = 1
}
