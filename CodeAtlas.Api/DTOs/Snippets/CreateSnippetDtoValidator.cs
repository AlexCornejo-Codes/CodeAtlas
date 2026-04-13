using FluentValidation;

namespace CodeAtlas.Api.DTOs.Snippets;

public sealed class CreateSnippetDtoValidator : AbstractValidator<CreateSnippetDto>
{
    public CreateSnippetDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100)
            .WithMessage("Title must be between 3 and 100 characters long");
        
        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must be less than 500 characters long");
        
        RuleFor(x => x.Code).
            NotEmpty()
            .MinimumLength(10)
            .MaximumLength(10000)
            .WithMessage("Code must be between 10 and 10000 characters long");
    }
}
