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
    
    public static void UpdateFromDto(this Technology technology, UpdateTechnologyDto dto)
    {
        technology.Name = dto.Name;
        technology.Description = dto.Description;
        technology.UpdatedAtUtc = DateTime.UtcNow;
    }
}
