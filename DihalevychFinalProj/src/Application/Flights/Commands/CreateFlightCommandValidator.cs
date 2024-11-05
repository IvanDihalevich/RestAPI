using FluentValidation;

namespace Application.Flights.Commands;

public class CreateFlightCommandValidator : AbstractValidator<CreateFlightCommand>
{
    public CreateFlightCommandValidator()
    {
        RuleFor(x => x.FlightName).NotEmpty().MaximumLength(255).MinimumLength(3);
        RuleFor(x => x.DepartureTime).NotEmpty().LessThan(x => x.ArrivalTime);
        RuleFor(x => x.ArrivalTime).NotEmpty().GreaterThan(x => x.DepartureTime);
        RuleFor(x => x.DepartureAirportId).NotEmpty();
        RuleFor(x => x.ArrivalAirportId).NotEmpty();
        RuleFor(x => x.AirplaneId).NotEmpty();
    }
}