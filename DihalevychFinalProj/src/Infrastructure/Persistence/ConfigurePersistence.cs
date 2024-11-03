using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Infrastructure.Persistence;

public static class ConfigurePersistence
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var dataSourceBuild = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("Default"));
        dataSourceBuild.EnableDynamicJson();
        var dataSource = dataSourceBuild.Build();

        services.AddDbContext<ApplicationDbContext>(
            options => options
                .UseNpgsql(
                    dataSource,
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                .UseSnakeCaseNamingConvention()
                .ConfigureWarnings(w => w.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning)));

        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<AirplaneRepository>();
        services.AddScoped<IAirplaneRepository>(provider => provider.GetRequiredService<AirplaneRepository>());
        services.AddScoped<IAirplaneQueries>(provider => provider.GetRequiredService<AirplaneRepository>());

        services.AddScoped<AirportRepository>();
        services.AddScoped<IAirportRepository>(provider => provider.GetRequiredService<AirportRepository>());
        services.AddScoped<IAirportQueries>(provider => provider.GetRequiredService<AirportRepository>());
        
        services.AddScoped<FlightRepository>();
        services.AddScoped<IFlightRepository>(provider => provider.GetRequiredService<FlightRepository>());
        services.AddScoped<IFlightQueries>(provider => provider.GetRequiredService<FlightRepository>());
        
        services.AddScoped<PassengerRepository>();
        services.AddScoped<IPassengerRepository>(provider => provider.GetRequiredService<PassengerRepository>());
        services.AddScoped<IPassengerQueries>(provider => provider.GetRequiredService<PassengerRepository>());
        
        services.AddScoped<TicketRepository>();
        services.AddScoped<ITicketRepository>(provider => provider.GetRequiredService<TicketRepository>());
        services.AddScoped<ITicketQueries>(provider => provider.GetRequiredService<TicketRepository>());
    }
}