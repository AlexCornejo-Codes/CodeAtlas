namespace CodeAtlas.Api.DTOs.SnippetTechnologies;

public sealed record UpsertSnippetTechnologiesDto
{
    public required List<string> TechnologyIds { get; init; }
}
