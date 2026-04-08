using CodeAtlas.Api.Database;
using CodeAtlas.Api.DTOs.Technologies;
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
}
