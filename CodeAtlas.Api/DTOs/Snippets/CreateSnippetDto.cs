using CodeAtlas.Api.Entities;

namespace CodeAtlas.Api.DTOs.Snippets;

public sealed record CreateSnippetDto
{
    public required string Title { get; init; }
    public string? Description { get; init; }
    public required string Code { get; init; } = string.Empty;
    public required Language Language { get; init; }
    public required Visibility Visibility { get; init; }
}
