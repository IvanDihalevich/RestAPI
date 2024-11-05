using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Airplanes.Exceptions;
using Domain.Airplanes;
using MediatR;

namespace Application.Airplanes.Commands;

public record UpdateAirplaneCommand : IRequest<Result<Airplane, AirplaneException>>
{
    public required Guid AirplaneId { get; init; }
    public required string Model { get; init; }
    public required int MaxPassenger { get; init; }
    public required int YearOfManufacture { get; init; }
}

public class UpdateAirplaneCommandHandler(IAirplaneRepository airplaneRepository) 
    : IRequestHandler<UpdateAirplaneCommand, Result<Airplane, AirplaneException>>
{
    public async Task<Result<Airplane, AirplaneException>> Handle(UpdateAirplaneCommand request, CancellationToken cancellationToken)
    {
        var airplaneId = new AirplaneId(request.AirplaneId);

        var existingAirplane = await airplaneRepository.GetById(airplaneId, cancellationToken);

        return await existingAirplane.Match(
            async a => await UpdateEntity(a, request.Model, request.MaxPassenger, request.YearOfManufacture, cancellationToken),
            () => Task.FromResult<Result<Airplane, AirplaneException>>(new AirplaneNotFoundException(airplaneId)));
    }

    private async Task<Result<Airplane, AirplaneException>> UpdateEntity(
        Airplane entity,
        string model,
        int maxPassenger,
        int yearOfManufacture,
        CancellationToken cancellationToken)
    {
        try
        {
            entity.UpdateDetails(model, maxPassenger, yearOfManufacture);

            return await airplaneRepository.Update(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new AirplaneUnknownException(entity.Id, exception);
        }
    }
}