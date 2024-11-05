using FluentValidation;

namespace Application.Airports.Commands;

public class CreateAirportCommandValidator : AbstractValidator<CreateAirportCommand>
{
    public CreateAirportCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255).MinimumLength(3);
        RuleFor(x => x.Location).NotEmpty().MaximumLength(255).MinimumLength(3);
    }
}