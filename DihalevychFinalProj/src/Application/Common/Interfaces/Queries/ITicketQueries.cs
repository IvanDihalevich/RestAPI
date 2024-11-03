using Domain.Tickets;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface ITicketQueries
{
    Task<IReadOnlyList<Ticket>> GetAll(CancellationToken cancellationToken);
    Task<Option<Ticket>> GetById(TicketId id, CancellationToken cancellationToken);
}