using CodeAtlas.Api.Database;
using CodeAtlas.Api.DTOs.Technologies;
using CodeAtlas.Api.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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
            Items = technologies
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
    public async Task<ActionResult<TechnologyDto>> CreateTechnology(
        CreateTechnologyDto createTechnologyDto,
        IValidator<CreateTechnologyDto> validator,
        ProblemDetailsFactory problemDetailsFactory)
    {
        ValidationResult validationResult = await validator.ValidateAsync(createTechnologyDto);
        if (!validationResult.IsValid)
        {
            ProblemDetails problem = problemDetailsFactory.CreateProblemDetails(
                HttpContext, StatusCodes.Status400BadRequest);
            problem.Extensions.Add("errors", validationResult.ToDictionary());
            
            return BadRequest(problem);
        }

        Technology technology = createTechnologyDto.ToEntity();
        
        if (await dbContext.Technologies.AnyAsync(t => t.Name == technology.Name))
        {
            return Problem(
                detail: $"The technology with name '{technology.Name}' already exists.",
                statusCode: StatusCodes.Status409Conflict);
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
