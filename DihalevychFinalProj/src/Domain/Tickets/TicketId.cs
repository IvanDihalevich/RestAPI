namespace Domain.Tickets;

public record TicketId(Guid Value)
{
    public static TicketId New() => new(Guid.NewGuid());
    public static TicketId Empty() => new(Guid.Empty);
    public override string ToString() => Value.ToString();
}