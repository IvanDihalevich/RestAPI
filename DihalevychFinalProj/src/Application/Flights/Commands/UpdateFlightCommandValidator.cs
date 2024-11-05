using FluentValidation;

namespace Application.Flights.Commands;

public class UpdateFlightCommandValidator : AbstractValidator<UpdateFlightCommand>
{
    public UpdateFlightCommandValidator()
    {
        RuleFor(x => x.FlightId).NotEmpty();
        RuleFor(x => x.FlightName).NotEmpty().MaximumLength(255).MinimumLength(3);
        RuleFor(x => x.DepartureTime).NotEmpty().LessThan(x => x.ArrivalTime);
        RuleFor(x => x.ArrivalTime).NotEmpty().GreaterThan(x => x.DepartureTime);
    }
}