using CodeAtlas.Api.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CodeAtlas.Api.DTOs.Snippets;

public sealed record SnippetsQueryParameters
{
    [FromQuery(Name = "q")]
    public string? Search { get; set; }
    public Language? Language { get; init; }
    public string? Sort { get; init; }
}
