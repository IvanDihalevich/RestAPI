using Domain.Airplanes;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IAirplaneQueries
{
    Task<IReadOnlyList<Airplane>> GetAll(CancellationToken cancellationToken);
    Task<Option<Airplane>> GetById(AirplaneId id, CancellationToken cancellationToken);
}