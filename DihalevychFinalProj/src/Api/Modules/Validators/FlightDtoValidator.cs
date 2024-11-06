using Api.Dtos.Flights;
using FluentValidation;

namespace Api.Modules.Validators;

public class FlightDtoValidator : AbstractValidator<FlightDto>
{
    public FlightDtoValidator()
    {
        RuleFor(x => x.FlightName).NotEmpty().MaximumLength(255).MinimumLength(3);
        RuleFor(x => x.DepartureTime).NotEmpty().LessThan(x => x.ArrivalTime);
        RuleFor(x => x.ArrivalTime).NotEmpty().GreaterThan(x => x.DepartureTime);
        RuleFor(x => x.DepartureAirportId).NotEmpty();
        RuleFor(x => x.ArrivalAirportId).NotEmpty();
        RuleFor(x => x.AirplaneId).NotEmpty();
    }
}
