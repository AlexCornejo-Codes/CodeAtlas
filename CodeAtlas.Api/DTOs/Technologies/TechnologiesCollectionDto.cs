using CodeAtlas.Api.DTOs.Common;

namespace CodeAtlas.Api.DTOs.Technologies;

public sealed record TechnologiesCollectionDto : ICollectionResponse<TechnologyDto>
{
    public List<TechnologyDto> Items { get; init; }
}
