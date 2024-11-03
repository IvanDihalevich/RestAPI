using Domain.Airplanes;

namespace Domain.Airports;

public class Airport
{
    public AirportId Id { get; }
    public string Name { get; private set; }
    public string Location { get; private set; }
    

    private Airport(AirportId id, string name, string location)
    {
        Id = id;
        Name = name;
        Location = location;
    }

    public static Airport New(AirportId id, string name, string location)
        => new(id, name, location);
    
    
}