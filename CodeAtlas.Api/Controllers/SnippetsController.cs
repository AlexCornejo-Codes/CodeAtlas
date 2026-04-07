using CodeAtlas.Api.Database;
using CodeAtlas.Api.DTOs.Snippets;
using CodeAtlas.Api.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeAtlas.Api.Controllers;

[ApiController]
[Route("snippets")]
public sealed class SnippetsController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<SnippetsCollectionDto>> GetSnippets()
    {
        List<SnippetDto> snippets = await dbContext
            .Snippets
            .Select(SnippetQueries.ProjectToDto())
            .ToListAsync();
        
        var snippetsCollectionDto = new SnippetsCollectionDto
        {
            Data = snippets
        };
        
        return Ok(snippetsCollectionDto);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<SnippetDto>> GetSnippet(string id)
    {
        SnippetDto? snippet = await dbContext
            .Snippets
            .Where(s => s.Id == id)
            .Select(SnippetQueries.ProjectToDto())
            .FirstOrDefaultAsync();

        if (snippet is null)
        {
            return NotFound();
        }
        
        return Ok(snippet);
    }

    [HttpPost]
    public async Task<ActionResult<SnippetDto>> CreateSnippet(CreateSnippetDto createSnippetDto)
    {
        Snippet snippet = createSnippetDto.ToEntity();
        
        dbContext.Snippets.Add(snippet);
        await dbContext.SaveChangesAsync();

        SnippetDto snippetDto = snippet.ToDto();
        
        return CreatedAtAction(nameof(GetSnippet), new { id = snippetDto.Id }, snippetDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateSnippet(string id, UpdateSnippetDto updateSnippetDto)
    {
        Snippet? snippet = await dbContext
            .Snippets
            .FirstOrDefaultAsync(s => s.Id == id);

        if (snippet is null)
        {
            return NotFound();
        }
        
        snippet.UpdateFromDto(updateSnippetDto);
        await dbContext.SaveChangesAsync();
        
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> ArchiveSnippet(string id, JsonPatchDocument<SnippetDto> patchDocument)
    {
        Snippet? snippet = await dbContext
            .Snippets
            .FirstOrDefaultAsync(s => s.Id == id);

        if (snippet is null)
        {
            return NotFound();
        }

        SnippetDto snippetDto = snippet.ToDto();
        
        patchDocument.ApplyTo(snippetDto, ModelState);
        
        if (!TryValidateModel(snippetDto))
        {
            return ValidationProblem(ModelState);
        }
        
        snippet.IsArchived = snippetDto.IsArchived;
        snippet.UpdatedAtUtc = DateTime.UtcNow;
        
        await dbContext.SaveChangesAsync();
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSnippet(string id)
    {
        Snippet? snippet = await dbContext
            .Snippets
            .FirstOrDefaultAsync(s => s.Id == id);

        if (snippet is null)
        {
            return NotFound();
        }
        
        dbContext.Snippets.Remove(snippet);
        await dbContext.SaveChangesAsync();
        
        return NoContent();
    }
}
