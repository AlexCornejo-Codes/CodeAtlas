using CodeAtlas.Api.Database;
using CodeAtlas.Api.DTOs.SnippetTechnologies;
using CodeAtlas.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeAtlas.Api.Controllers;

[ApiController]
[Route("snippets/{snippetId}/technologies")]
public sealed class SnippetTechnologiesController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpPut]
    public async Task<ActionResult> UpsertSnippetTechnologies(string snippetId,
        UpsertSnippetTechnologiesDto upsertSnippetTechnologiesDto)
    {
        Snippet? snippet = await dbContext.Snippets
            .Include(s => s.SnippetTechnologies)
            .FirstOrDefaultAsync(s => s.Id == snippetId);

        if (snippet is null)
        {
            return NotFound();
        }

        var currentTechnologyIds = snippet.SnippetTechnologies.Select(st => st.TechnologyId).ToHashSet();
        if (currentTechnologyIds.SetEquals(upsertSnippetTechnologiesDto.TechnologyIds))
        {
            return NoContent();
        }
        
        List<string> existingTechnologyIds = await dbContext
            .Technologies
            .Where(t => upsertSnippetTechnologiesDto.TechnologyIds.Contains(t.Id))
            .Select(t => t.Id)
            .ToListAsync();

        if (existingTechnologyIds.Count != upsertSnippetTechnologiesDto.TechnologyIds.Count)
        {
            return BadRequest("One or more technology IDs are invalid.");
        }
        
        snippet.SnippetTechnologies.RemoveAll(st =>
            !upsertSnippetTechnologiesDto.TechnologyIds.Contains(st.TechnologyId));

        string[] technologyIdsToAdd = upsertSnippetTechnologiesDto.TechnologyIds.Except(currentTechnologyIds).ToArray();
        snippet.SnippetTechnologies.AddRange(technologyIdsToAdd.Select(technologyId => new SnippetTechnology
        {
            SnippetId = snippetId,
            TechnologyId = technologyId,
            CreatedAtUtc = DateTime.UtcNow
        }));
        
        await dbContext.SaveChangesAsync();
        
        return NoContent();
    }
    
    [HttpDelete("{technologyId}")]
    public async Task<ActionResult> DeleteSnippetTechnology(string snippetId, string technologyId)
    {
        SnippetTechnology? snippetTechnology = await dbContext.SnippetTechnologies
            .SingleOrDefaultAsync(st => st.SnippetId == snippetId && st.TechnologyId == technologyId);

        if (snippetTechnology is null)
        {
            return NotFound();
        }
        
        dbContext.SnippetTechnologies.Remove(snippetTechnology);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}
