using CodeAtlas.Api.Database;
using CodeAtlas.Api.DTOs.Snippets;
using CodeAtlas.Api.Entities;
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
}
