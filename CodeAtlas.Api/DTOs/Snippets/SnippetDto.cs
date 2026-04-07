using CodeAtlas.Api.Entities;

namespace CodeAtlas.Api.DTOs.Snippets;

public sealed record SnippetsCollectionDto
{
    public List<SnippetDto> Data { get; init; }
}

public sealed record SnippetDto
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
    public required string Code { get; init; } = string.Empty;
    public required Language Language { get; init; }
    public required Visibility Visibility { get; init; }
    public required bool IsArchived { get; init; }
    public required DateTime CreatedAtUtc { get; init; }
    public DateTime? UpdatedAtUtc { get; init; }
}
