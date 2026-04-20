using System.Dynamic;
using CodeAtlas.Api.Database;
using CodeAtlas.Api.DTOs.Common;
using CodeAtlas.Api.DTOs.Snippets;
using CodeAtlas.Api.Entities;
using CodeAtlas.Api.Services;
using CodeAtlas.Api.Services.Sorting;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeAtlas.Api.Controllers;

[ApiController]
[Route("snippets")]
public sealed class SnippetsController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetSnippets(
        [FromQuery] SnippetsQueryParameters query,
        SortMappingProvider sortMappingProvider,
        DataShapingService dataShapingService)
    {
        if (!sortMappingProvider.ValidateMappings<SnippetDto, Snippet>(query.Sort))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                detail: $"The provided sort parameter '{query.Sort}' is not valid." +
                        $" Please check the documentation for valid sort parameters.");
        }

        if (!dataShapingService.Validate<SnippetDto>(query.Fields))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                detail: $"The provided data shaping fields are not valid: '{query.Fields}'");
        }

        query.Search ??= query.Search?.Trim().ToLower();

        SortMapping[] sortMappings = sortMappingProvider.GetMappings<SnippetDto, Snippet>();

        IQueryable<SnippetDto> snippetsQuery = dbContext
            .Snippets
            .Where(s => query.Search == null || 
                        s.Title.ToLower().Contains(query.Search) || 
                        s.Description != null && s.Description.ToLower().Contains(query.Search))
            .Where(s => query.Language == null || s.Language == query.Language)
            .ApplySort(query.Sort, sortMappings)
            .Select(SnippetQueries.ProjectToDto());
        
        var totalCount = await snippetsQuery.CountAsync();
        
        List<SnippetDto> snippets = await snippetsQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        var paginationResult = new PaginationResult<ExpandoObject>
        {
            Items = dataShapingService.ShapeCollectionData(snippets, query.Fields),
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
        
        return Ok(paginationResult);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSnippet(
        string id,
        string? fields,
        DataShapingService dataShapingService)
    {
        if (!dataShapingService.Validate<SnippetDto>(fields))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                detail: $"The provided data shaping fields are not valid: '{fields}'");
        }
        
        SnippetWithTechnologiesDto? snippet = await dbContext
            .Snippets
            .Where(s => s.Id == id)
            .Select(SnippetQueries.ProjectToDtoWithTechnologiesDto())
            .FirstOrDefaultAsync();

        if (snippet is null)
        {
            return NotFound();
        }
        
        ExpandoObject shapedSnippetDto = dataShapingService.ShapeData(snippet, fields);
        
        return Ok(shapedSnippetDto);
    }

    [HttpPost]
    public async Task<ActionResult<SnippetDto>> CreateSnippet(CreateSnippetDto createSnippetDto,
        IValidator<CreateSnippetDto> validator)
    {
        await validator.ValidateAndThrowAsync(createSnippetDto);
        
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
