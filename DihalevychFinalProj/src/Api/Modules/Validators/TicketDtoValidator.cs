using Api.Dtos.Tickets;
using FluentValidation;

namespace Api.Modules.Validators;

public class TicketDtoValidator : AbstractValidator<TicketDto>
{
    public TicketDtoValidator()
    {
        RuleFor(x => x.FlightId).NotEmpty().WithMessage("Flight ID is required.");
        RuleFor(x => x.PassengerId).NotEmpty().WithMessage("Passenger ID is required.");
    }
}
