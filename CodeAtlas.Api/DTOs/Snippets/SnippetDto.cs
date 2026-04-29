using CodeAtlas.Api.DTOs.Common;
using CodeAtlas.Api.Entities;

namespace CodeAtlas.Api.DTOs.Snippets;

public sealed record SnippetDto : ILinksResponse
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
    public List<LinkDto> Links { get; set; }
}
