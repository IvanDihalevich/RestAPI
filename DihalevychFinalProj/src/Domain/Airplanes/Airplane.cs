using Domain.Airports;

namespace Domain.Airplanes;

public class Airplane
{
    public AirplaneId Id { get; }
    public string Model { get; private set; }
    public int MaxPassenger { get; private set; }
    public int YearOfManufacture { get; private set; }
    public Airport? Airport { get; private set; }
    public AirportId AirportId { get; private set; }

    private Airplane(AirplaneId id, string model, int maxPassenger, int yearOfManufacture, AirportId airportId)
    {
        Id = id;
        Model = model;
        MaxPassenger = maxPassenger;
        YearOfManufacture = yearOfManufacture;
        AirportId = airportId;
    }

    public static Airplane New(AirplaneId id, string model, int capacity, int yearOfManufacture,AirportId airportId)
        => new(id, model, capacity, yearOfManufacture, airportId);
    
    public void UpdateDetails(string model, int maxPassenger, int yearOfManufacture)
    {
        Model = model;
        MaxPassenger = maxPassenger;
        YearOfManufacture = yearOfManufacture;
    }

}