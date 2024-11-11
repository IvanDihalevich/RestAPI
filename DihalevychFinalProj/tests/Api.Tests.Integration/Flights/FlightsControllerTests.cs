using System.Net;
using System.Net.Http.Json;
using Api.Dtos.Flights;
using Domain.Airplanes;
using Domain.Airports;
using Domain.Flights;
using FluentAssertions;
using Tests.Common;
using Tests.Data;
using Xunit;

namespace Api.Tests.Integration.Flights;

public class FlightsControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Airport _departureAirport;
    private readonly Airport _arrivalAirport;
    private readonly Airplane _airplane;
    private readonly Flight _mainFlight;

    public FlightsControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
        _departureAirport = AirportsData.MainAirport();
        _arrivalAirport = AirportsData.MainAirport();
        _airplane = AirplanesData.MainAirplane(_departureAirport.Id);
        _mainFlight = FlightsData.MainFlight(_departureAirport.Id, _arrivalAirport.Id, _airplane.Id);
    }

    [Fact]
    public async Task ShouldCreateFlight()
    {
        // Arrange
        var request = new CreateFlightDto(
            FlightName: "New Flight",
            DepartureTime: DateTime.UtcNow.AddHours(2),
            ArrivalTime: DateTime.UtcNow.AddHours(6),
            DepartureAirportId: _departureAirport.Id.Value,
            ArrivalAirportId: _arrivalAirport.Id.Value,
            AirplaneId: _airplane.Id.Value);

        // Act
        var response = await Client.PostAsJsonAsync("flights/create", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseFlight = await response.Content.ReadFromJsonAsync<CreateFlightDto>();
        responseFlight.Should().NotBeNull();
        responseFlight!.FlightName.Should().Be("New Flight");
    }

    [Fact]
    public async Task ShouldGetAllFlights()
    {
        // Act
        var response = await Client.GetAsync("flights/getAll");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var flights = await response.Content.ReadFromJsonAsync<IReadOnlyList<FlightDto>>();
        flights.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task ShouldGetFlightById()
    {
        // Act
        var response = await Client.GetAsync($"flights/{_mainFlight.Id.Value}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var flight = await response.Content.ReadFromJsonAsync<FlightDto>();
        flight.Should().NotBeNull();
        flight!.Id.Should().Be(_mainFlight.Id.Value);
    }

    [Fact]
    public async Task GetFlightById_ShouldReturnNotFound_WhenIdIsInvalid()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync($"flights/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldUpdateFlight()
    {
        // Arrange
        var updateRequest = new UpdateFlightDto(
            FlightName: "Updated Flight",
            DepartureTime: DateTime.UtcNow.AddHours(3),
            ArrivalTime: DateTime.UtcNow.AddHours(7));

        // Act
        var response = await Client.PutAsJsonAsync($"flights/{_mainFlight.Id.Value}", updateRequest);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var updatedFlight = await response.Content.ReadFromJsonAsync<FlightDto>();
        updatedFlight.Should().NotBeNull();
        updatedFlight!.FlightName.Should().Be(updateRequest.FlightName);
    }

    [Fact]
    public async Task UpdateFlight_ShouldReturnBadRequest_WhenDepartureTimeIsAfterArrivalTime()
    {
        // Arrange
        var updateRequest = new UpdateFlightDto(
            FlightName: "Invalid Flight",
            DepartureTime: DateTime.UtcNow.AddHours(5),
            ArrivalTime: DateTime.UtcNow.AddHours(3)); // час

        // Act
        var response = await Client.PutAsJsonAsync($"flights/{_mainFlight.Id.Value}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }


    [Fact]
    public async Task ShouldDeleteFlight()
    {
        // Act
        var response = await Client.DeleteAsync($"flights/{_mainFlight.Id.Value}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var deletedFlight = await Context.Flights.FindAsync(_mainFlight.Id);
        deletedFlight.Should().BeNull();
    }

    [Fact]
    public async Task DeleteFlight_ShouldReturnNotFound_WhenIdIsInvalid()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act
        var response = await Client.DeleteAsync($"flights/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateFlight_ShouldReturnError_WhenDataIsInvalid()
    {
        // Arrange
        var request = new CreateFlightDto(
            FlightName: "",
            DepartureTime: DateTime.UtcNow,
            ArrivalTime: DateTime.UtcNow.AddHours(-1),
            DepartureAirportId: Guid.NewGuid(),
            ArrivalAirportId: Guid.NewGuid(),
            AirplaneId: Guid.NewGuid());

        // Act
        var response = await Client.PostAsJsonAsync("flights/create", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
    }

    [Fact]
    public async Task CreateFlight_ShouldReturnBadRequest_WhenFlightNameIsMissing()
    {
        // Arrange
        var request = new CreateFlightDto(
            FlightName: "",
            DepartureTime: DateTime.UtcNow.AddHours(1),
            ArrivalTime: DateTime.UtcNow.AddHours(5),
            DepartureAirportId: _departureAirport.Id.Value,
            ArrivalAirportId: _arrivalAirport.Id.Value,
            AirplaneId: _airplane.Id.Value);

        // Act
        var response = await Client.PostAsJsonAsync("flights/create", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    public async Task InitializeAsync()
    {
        await Context.Airports.AddRangeAsync(_departureAirport, _arrivalAirport);
        await Context.Airplanes.AddAsync(_airplane);
        await Context.Flights.AddAsync(_mainFlight);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Flights.RemoveRange(Context.Flights);
        Context.Airplanes.RemoveRange(Context.Airplanes);
        Context.Airports.RemoveRange(Context.Airports);
        await SaveChangesAsync();
    }
}
