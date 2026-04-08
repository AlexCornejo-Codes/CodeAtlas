using CodeAtlas.Api.Entities;

namespace CodeAtlas.Api.DTOs.Technologies;

internal static class TechnologyMappings
{
    public static TechnologyDto ToDto(this Technology technology)
    {
        return new TechnologyDto
        {
            Id = technology.Id,
            Name = technology.Name,
            Description = technology.Description,
            CreatedAtUtc = technology.CreatedAtUtc,
            UpdatedAtUtc = technology.UpdatedAtUtc
        };
    }
    
    public static Technology ToEntity(this CreateTechnologyDto dto)
    {
        Technology technology = new()
        {
            Id = $"t_{Guid.CreateVersion7()}",
            Name = dto.Name,
            Description = dto.Description,
            CreatedAtUtc = DateTime.UtcNow
        };
        
        return technology;
    }
}
