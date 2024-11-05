using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Airplanes.Exceptions;
using Domain.Airports;
using Domain.Airplanes;
using MediatR;

namespace Application.Airplanes.Commands;

public record CreateAirplaneCommand : IRequest<Result<Airplane, AirplaneException>>
{
    public required string Model { get; init; }
    public required int MaxPassenger { get; init; }
    public required int YearOfManufacture { get; init; }
    public required Guid AirportId { get; init; }
}

public class CreateAirplaneCommandHandler(
    IAirplaneRepository airplaneRepository,
    IAirportRepository airportRepository)
    : IRequestHandler<CreateAirplaneCommand, Result<Airplane, AirplaneException>>
{
    public async Task<Result<Airplane, AirplaneException>> Handle(CreateAirplaneCommand request, CancellationToken cancellationToken)
    {
        var airportId = new AirportId(request.AirportId);

        var airport = await airportRepository.GetById(airportId, cancellationToken);

        return await airport.Match<Task<Result<Airplane, AirplaneException>>>(
            async a => await CreateEntity(request.Model, request.MaxPassenger, request.YearOfManufacture, a.Id, cancellationToken),
            () => Task.FromResult<Result<Airplane, AirplaneException>>(new AirportNotFoundException(airportId)));
    }

    private async Task<Result<Airplane, AirplaneException>> CreateEntity(
        string model,
        int maxPassenger,
        int yearOfManufacture,
        AirportId airportId,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = Airplane.New(AirplaneId.New(), model, maxPassenger, yearOfManufacture, airportId);

            return await airplaneRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new AirplaneUnknownException(AirplaneId.Empty(), exception);
        }
    }
}