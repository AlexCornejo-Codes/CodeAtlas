using CodeAtlas.Api.Entities;

namespace CodeAtlas.Api.DTOs.Snippets;

internal static class SnippetMappings
{
    public static SnippetDto ToDto(this Snippet snippet)
    {
        return new SnippetDto
        {
            Id = snippet.Id,
            Title = snippet.Title,
            Description = snippet.Description,
            Code = snippet.Code,
            Language = snippet.Language,
            Visibility = snippet.Visibility,
            IsArchived = snippet.IsArchived,
            CreatedAtUtc = snippet.CreatedAtUtc,
            UpdatedAtUtc = snippet.UpdatedAtUtc
        };
    }
    
    public static Snippet ToEntity(this CreateSnippetDto dto)
    {
        Snippet snippet = new()
        {
            Id = $"s_{Guid.CreateVersion7()}",
            Title = dto.Title,
            Description = dto.Description,
            Code = dto.Code,
            Language = dto.Language,
            Visibility = dto.Visibility,
            IsArchived = false,
            CreatedAtUtc = DateTime.UtcNow
        };
        
        return snippet;
    }
}
