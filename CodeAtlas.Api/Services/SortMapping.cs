namespace CodeAtlas.Api.Services;

public sealed record SortMapping(string SortField, string PropertyName, bool Reverse = false);
