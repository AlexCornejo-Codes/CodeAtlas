using System.Linq.Expressions;
using CodeAtlas.Api.Entities;

namespace CodeAtlas.Api.DTOs.Technologies;

internal static class TechnologyQueries
{
    public static Expression<Func<Technology, TechnologyDto>> ProjectToDto()
    {
        return t => new TechnologyDto
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Description,
            CreatedAtUtc = t.CreatedAtUtc,
            UpdatedAtUtc = t.UpdatedAtUtc
        };
    }
}
