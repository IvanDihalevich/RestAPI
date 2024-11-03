using Domain.Flights;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IFlightQueries
{
    Task<IReadOnlyList<Flight>> GetAll(CancellationToken cancellationToken);
    Task<Option<Flight>> GetById(FlightId id, CancellationToken cancellationToken);
}