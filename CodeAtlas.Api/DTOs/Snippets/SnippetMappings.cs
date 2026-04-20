using CodeAtlas.Api.Entities;
using CodeAtlas.Api.Services;
using CodeAtlas.Api.Services.Sorting;

namespace CodeAtlas.Api.DTOs.Snippets;

internal static class SnippetMappings
{
    public static readonly SortMappingDefinition<SnippetDto, Snippet> SortMapping = new()
    {
        Mappings =
        [
            new SortMapping("title", nameof(Snippet.Title)),
            new SortMapping("description", nameof(Snippet.Description)),
            new SortMapping("language", nameof(Snippet.Language)),
            new SortMapping("visibility", nameof(Snippet.Visibility)),
            new SortMapping("createdAtUtc", nameof(Snippet.CreatedAtUtc)),
            new SortMapping("updatedAtUtc", nameof(Snippet.UpdatedAtUtc))
        ]
    };
        
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
    
    public static void UpdateFromDto(this Snippet snippet, UpdateSnippetDto dto)
    {
        snippet.Title = dto.Title;
        snippet.Description = dto.Description;
        snippet.Code = dto.Code;
        snippet.Language = dto.Language;
        snippet.Visibility = dto.Visibility;
        snippet.UpdatedAtUtc = DateTime.UtcNow;
    }
    
}
