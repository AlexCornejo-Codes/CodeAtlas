using FluentValidation;

namespace CodeAtlas.Api.DTOs.Technologies;

public sealed record CreateTechnologyDto
{
    public required string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
}

public sealed class CreateTechnologyDtoValidator : AbstractValidator<CreateTechnologyDto>
{
    public CreateTechnologyDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3);
        RuleFor(x => x.Description).MaximumLength(50);
    }
}
