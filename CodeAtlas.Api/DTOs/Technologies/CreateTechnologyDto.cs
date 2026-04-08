namespace CodeAtlas.Api.DTOs.Technologies;

public sealed record CreateTechnologyDto
{
    public required string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
}
