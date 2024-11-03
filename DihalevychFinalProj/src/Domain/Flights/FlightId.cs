namespace Domain.Flights;

public record FlightId(Guid Value)
{
    public static FlightId New() => new(Guid.NewGuid());
    public static FlightId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}