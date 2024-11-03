namespace Domain.Airports;

public record AirportId(Guid Value)
{
    public static AirportId New() => new(Guid.NewGuid());
    public static AirportId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}