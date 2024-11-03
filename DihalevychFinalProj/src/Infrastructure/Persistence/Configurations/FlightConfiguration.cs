using Domain.Airplanes;
using Domain.Airports;
using Domain.Flights;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class FlightConfiguration : IEntityTypeConfiguration<Flight>
{
    public void Configure(EntityTypeBuilder<Flight> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new FlightId(x));

        builder.Property(x => x.FlightName).IsRequired().HasColumnType("varchar(255)");
        
        builder.Property(x => x.DepartureTime)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())");
        
        builder.Property(x => x.ArrivalTime)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())");

        builder.Property(x => x.DepartureAirportId)
            .HasConversion(x => x.Value, x => new AirportId(x))
            .IsRequired();
        builder.HasOne(x => x.DepartureAirport)
            .WithMany()
            .HasForeignKey(x => x.DepartureAirportId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.ArrivalAirportId)
            .HasConversion(x => x.Value, x => new AirportId(x))
            .IsRequired();
        builder.HasOne(x => x.ArrivalAirport)
            .WithMany()
            .HasForeignKey(x => x.ArrivalAirportId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(x => x.AirplaneId)
            .HasConversion(x => x.Value, x => new AirplaneId(x))
            .IsRequired();
        builder.HasOne(x => x.Airplane)
            .WithMany()
            .HasForeignKey(x => x.AirplaneId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}