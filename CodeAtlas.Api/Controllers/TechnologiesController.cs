using CodeAtlas.Api.Database;
using CodeAtlas.Api.DTOs.Technologies;
using CodeAtlas.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeAtlas.Api.Controllers;

[ApiController]
[Route("technologies")]
public sealed class TechnologiesController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<TechnologiesCollectionDto>> GetTechnologies()
    {
        List<TechnologyDto> technologies = await dbContext
            .Technologies
            .Select( TechnologyQueries.ProjectToDto() )
            .ToListAsync();
        
        var technologiesCollectionDto = new TechnologiesCollectionDto
        {
            Data = technologies
        };
        
        return Ok(technologiesCollectionDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TechnologyDto>> GetTechnology(string id)
    {
        TechnologyDto? technology = await dbContext
            .Technologies
            .Where(t => t.Id == id)
            .Select( TechnologyQueries.ProjectToDto() )
            .FirstOrDefaultAsync();

        if (technology is null)
        {
            return NotFound();
        }
         
        return Ok(technology);
    }
    
    [HttpPost]
    public async Task<ActionResult<TechnologyDto>> CreateTechnology(CreateTechnologyDto createTechnologyDto)
    {
        Technology technology = createTechnologyDto.ToEntity();
        
        if (await dbContext.Technologies.AnyAsync(t => t.Name == technology.Name))
        {
            return Conflict($"The technology {technology.Name} already exists.");
        }
        
        dbContext.Technologies.Add(technology);
        await dbContext.SaveChangesAsync();
        
        TechnologyDto technologyDto = technology.ToDto();
        
        return CreatedAtAction(nameof(GetTechnology), new { id = technologyDto.Id }, technologyDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTechnology(string id, UpdateTechnologyDto updateTechnologyDto)
    {
        Technology? technology = await dbContext
            .Technologies
            .FirstOrDefaultAsync(t => t.Id == id);

        if (technology is null)
        {
            return NotFound();
        }
        
        technology.UpdateFromDto(updateTechnologyDto);
        await dbContext.SaveChangesAsync();
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTechnology(string id)
    {
        Technology? technology = await dbContext
            .Technologies
            .FirstOrDefaultAsync(t => t.Id == id);

        if (technology is null)
        {
            return NotFound();
        }
        
        dbContext.Technologies.Remove(technology);
        await dbContext.SaveChangesAsync();
        
        return NoContent();
    }
}
