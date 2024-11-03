using Domain.Flights;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IFlightRepository
{
    Task<Flight> Add(Flight flight, CancellationToken cancellationToken);
    Task<Flight> Update(Flight flight, CancellationToken cancellationToken);
    Task<Flight> Delete(Flight flight, CancellationToken cancellationToken);
    Task<Option<Flight>> GetById(FlightId id, CancellationToken cancellationToken);
}