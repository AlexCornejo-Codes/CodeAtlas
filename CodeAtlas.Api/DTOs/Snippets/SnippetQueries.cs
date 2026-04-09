using System.Linq.Expressions;
using CodeAtlas.Api.Entities;

namespace CodeAtlas.Api.DTOs.Snippets;

internal static class SnippetQueries
{
    public static Expression<Func<Snippet, SnippetDto>> ProjectToDto()
    {
        return s => new SnippetDto
        {
            Id = s.Id,
            Title = s.Title,
            Description = s.Description,
            Code = s.Code,
            Language = s.Language,
            Visibility = s.Visibility,
            IsArchived = s.IsArchived,
            CreatedAtUtc = s.CreatedAtUtc,
            UpdatedAtUtc = s.UpdatedAtUtc
        };
    }
    
    public static Expression<Func<Snippet, SnippetWithTechnologiesDto>> ProjectToDtoWithTechnologiesDto()
    {
        return s => new SnippetWithTechnologiesDto()
        {
            Id = s.Id,
            Title = s.Title,
            Description = s.Description,
            Code = s.Code,
            Language = s.Language,
            Visibility = s.Visibility,
            IsArchived = s.IsArchived,
            CreatedAtUtc = s.CreatedAtUtc,
            UpdatedAtUtc = s.UpdatedAtUtc,
            Technologies = s.Technologies.Select(t => t.Name).ToArray()
        };
    }
}
