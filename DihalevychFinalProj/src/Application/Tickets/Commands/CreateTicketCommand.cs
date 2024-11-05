using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Tickets.Exceptions;
using Domain.Flights;
using Domain.Passengers;
using Domain.Tickets;
using MediatR;

namespace Application.Tickets.Commands;

public record CreateTicketCommand : IRequest<Result<Ticket, TicketException>>
{
    public required Guid FlightId { get; init; }
    public required Guid PassengerId { get; init; }
}

public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, Result<Ticket, TicketException>>
{
    private readonly ITicketRepository _ticketRepository;

    public CreateTicketCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<Ticket, TicketException>> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = Ticket.New(TicketId.New(), new FlightId(request.FlightId), new PassengerId(request.PassengerId));

        try
        {
            return await _ticketRepository.Add(ticket, cancellationToken);
        }
        catch (Exception ex)
        {
            return new TicketUnknownException(ticket.Id, ex);
        }
    }
}