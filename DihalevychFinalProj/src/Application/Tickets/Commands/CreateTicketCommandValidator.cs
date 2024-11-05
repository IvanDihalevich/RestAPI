using FluentValidation;

namespace Application.Tickets.Commands;

public class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketCommandValidator()
    {
        RuleFor(x => x.FlightId).NotEmpty().WithMessage("Flight ID is required.");
        RuleFor(x => x.PassengerId).NotEmpty().WithMessage("Passenger ID is required.");
    }
}