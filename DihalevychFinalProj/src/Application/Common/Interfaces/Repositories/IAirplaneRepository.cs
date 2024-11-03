using Domain.Airplanes;
using Optional;

namespace Application.Common.Interfaces.Repositories;
    
public interface IAirplaneRepository
{
    Task<Airplane> Add(Airplane airplane, CancellationToken cancellationToken);
    Task<Airplane> Update(Airplane airplane, CancellationToken cancellationToken);
    Task<Airplane> Delete(Airplane airplane, CancellationToken cancellationToken);
    Task<Option<Airplane>> GetById(AirplaneId id, CancellationToken cancellationToken);
}