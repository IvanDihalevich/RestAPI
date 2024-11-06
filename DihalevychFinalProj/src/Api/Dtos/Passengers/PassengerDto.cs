using Domain.Passengers;

namespace Api.Dtos.Passengers;


public record PassengerDto(
    Guid? Id,
    string FirstName,
    string LastName,
    int Age)
{
    public static PassengerDto FromDomainModel(Passenger passenger)
        => new(
            Id: passenger.Id.Value,
            FirstName: passenger.FirstName,
            LastName: passenger.LastName,
            Age: passenger.Age);
}
