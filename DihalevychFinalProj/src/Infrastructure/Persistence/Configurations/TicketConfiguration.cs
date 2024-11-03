using Domain.Flights;
using Domain.Passengers;
using Domain.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new TicketId(x));

        builder.Property(x => x.FlightId)
            .HasConversion(x => x.Value, x => new FlightId(x))
            .IsRequired();
        builder.HasOne(x => x.Flight)
            .WithMany()
            .HasForeignKey(x => x.FlightId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(x => x.PassengerId)
            .HasConversion(x => x.Value, x => new PassengerId(x))
            .IsRequired();
        builder.HasOne(x => x.Passenger)
            .WithMany()
            .HasForeignKey(x => x.PassengerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(x => x.PurchaseDate).IsRequired();
    }
}