using Domain.Passengers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PassengerConfiguration : IEntityTypeConfiguration<Passenger>
{
    public void Configure(EntityTypeBuilder<Passenger> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new PassengerId(x));

        builder.Property(x => x.FirstName).IsRequired().HasColumnType("varchar(255)");
        builder.Property(x => x.LastName).IsRequired().HasColumnType("varchar(255)");
        builder.Property(x => x.Age).IsRequired();
    }
}