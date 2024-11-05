using FluentValidation;

namespace Application.Airports.Commands;

public class DeleteAirportCommandValidator : AbstractValidator<DeleteAirportCommand>
{
    public DeleteAirportCommandValidator()
    {
        RuleFor(x => x.AirportId).NotEmpty();
    }
}