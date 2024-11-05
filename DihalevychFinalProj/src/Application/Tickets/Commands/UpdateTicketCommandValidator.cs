using FluentValidation;

namespace Application.Tickets.Commands;

public class UpdateTicketCommandValidator : AbstractValidator<UpdateTicketCommand>
{
    public UpdateTicketCommandValidator()
    {
        RuleFor(x => x.TicketId).NotEmpty().WithMessage("Ticket ID is required.");
        RuleFor(x => x.FlightId).NotEmpty().WithMessage("Flight ID is required.");
        RuleFor(x => x.PassengerId).NotEmpty().WithMessage("Passenger ID is required.");
    }
}