using FluentValidation;

namespace Write.Api.Application.Products.Create;

public class Validator : AbstractValidator<Command>
{
    public Validator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(c => c.Barcode)
            .NotEmpty()
            .Length(8, 14)
            .Matches("^[a-zA-Z0-9]+$").WithMessage("Barcode must be alphanumeric.");

        RuleFor(c => c.Color)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(c => c.Size)
            .NotEmpty()
            .MaximumLength(50);
    }
}
