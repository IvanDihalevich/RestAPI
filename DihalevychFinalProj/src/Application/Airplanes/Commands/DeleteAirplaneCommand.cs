using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Airplanes.Exceptions;
using Domain.Airplanes;
using MediatR;

namespace Application.Airplanes.Commands;

public record DeleteAirplaneCommand : IRequest<Result<Airplane, AirplaneException>>
{
    public required Guid AirplaneId { get; init; }
}

public class DeleteAirplaneCommandHandler(IAirplaneRepository airplaneRepository)
    : IRequestHandler<DeleteAirplaneCommand, Result<Airplane, AirplaneException>>
{
    public async Task<Result<Airplane, AirplaneException>> Handle(
        DeleteAirplaneCommand request,
        CancellationToken cancellationToken)
    {
        var airplaneId = new AirplaneId(request.AirplaneId);

        var existingAirplane = await airplaneRepository.GetById(airplaneId, cancellationToken);

        return await existingAirplane.Match<Task<Result<Airplane, AirplaneException>>>(
            async a => await DeleteEntity(a, cancellationToken),
            () => Task.FromResult<Result<Airplane, AirplaneException>>(new AirplaneNotFoundException(airplaneId)));
    }

    public async Task<Result<Airplane, AirplaneException>> DeleteEntity(Airplane airplane, CancellationToken cancellationToken)
    {
        try
        {
            return await airplaneRepository.Delete(airplane, cancellationToken);
        }
        catch (Exception exception)
        {
            return new AirplaneUnknownException(airplane.Id, exception);
        }
    }
}