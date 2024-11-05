using FluentValidation;

namespace Application.Flights.Commands;

public class DeleteFlightCommandValidator : AbstractValidator<DeleteFlightCommand>
{
    public DeleteFlightCommandValidator()
    {
        RuleFor(x => x.FlightId).NotEmpty();
    }
}
