using FluentValidation;

namespace Application.Airports.Commands;

public class UpdateAirportCommandValidator : AbstractValidator<UpdateAirportCommand>
{
    public UpdateAirportCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255).MinimumLength(3);
        RuleFor(x => x.Location).NotEmpty().MaximumLength(255).MinimumLength(3);
        RuleFor(x => x.AirportId).NotEmpty();
    }
}