using Domain.Tickets;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface ITicketRepository
{
    Task<Ticket> Add(Ticket ticket, CancellationToken cancellationToken);
    Task<Ticket> Update(Ticket ticket, CancellationToken cancellationToken);
    Task<Ticket> Delete(Ticket ticket, CancellationToken cancellationToken);
    Task<Option<Ticket>> GetById(TicketId id, CancellationToken cancellationToken);
}