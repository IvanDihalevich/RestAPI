using Domain.Airports;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IAirportRepository
{
    Task<Airport> Add(Airport airport, CancellationToken cancellationToken);
    Task<Airport> Update(Airport airport, CancellationToken cancellationToken);
    Task<Airport> Delete(Airport airport, CancellationToken cancellationToken);
    Task<Option<Airport>> GetById(AirportId id, CancellationToken cancellationToken);
}