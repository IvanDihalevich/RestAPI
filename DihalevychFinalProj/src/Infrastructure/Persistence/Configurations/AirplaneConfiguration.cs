using Domain.Airplanes;
using Domain.Airports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class AirplaneConfiguration : IEntityTypeConfiguration<Airplane>
{
    public void Configure(EntityTypeBuilder<Airplane> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new AirplaneId(x));

        builder.Property(x => x.Model).IsRequired().HasColumnType("varchar(255)");
        builder.Property(x => x.MaxPassenger).IsRequired();
        builder.Property(x => x.YearOfManufacture).IsRequired();

        builder.Property(x => x.AirportId)
            .HasConversion(x => x.Value, x => new AirportId(x))
            .IsRequired();
        builder.HasOne(x => x.Airport)
            .WithMany()
            .HasForeignKey(x => x.AirportId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}