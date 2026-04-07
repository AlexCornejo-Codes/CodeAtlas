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
            .Select(s => new SnippetDto
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                Code = s.Code,
                Language = s.Language,
                Visibility = s.Visibility,
                IsArchived = s.IsArchived,
                CreatedAtUtc = s.CreatedAtUtc,
                UpdatedAtUtc = s.UpdatedAtUtc
            })
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
            .Select(s => new SnippetDto
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                Code = s.Code,
                Language = s.Language,
                Visibility = s.Visibility,
                IsArchived = s.IsArchived,
                CreatedAtUtc = s.CreatedAtUtc,
                UpdatedAtUtc = s.UpdatedAtUtc
            })
            .FirstOrDefaultAsync();

        if (snippet is null)
        {
            return NotFound();
        }
        
        return Ok(snippet);
    }
}
