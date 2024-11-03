using Domain.Airports;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IAirportQueries
{
    Task<IReadOnlyList<Airport>> GetAll(CancellationToken cancellationToken);
    Task<Option<Airport>> GetById(AirportId id, CancellationToken cancellationToken);
}