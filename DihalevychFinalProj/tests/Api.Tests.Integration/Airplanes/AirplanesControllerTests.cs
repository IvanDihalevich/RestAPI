using System.Net;
using System.Net.Http.Json;
using Api.Dtos.Airplanes;
using Domain.Airplanes;
using Domain.Airports;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data;
using Xunit;

namespace Api.Tests.Integration.Airplanes;

public class AirplanesControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Airport _mainAirport;
    private readonly Airplane _mainAirplane;

    public AirplanesControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
        _mainAirport = AirportsData.MainAirport();
        _mainAirplane = AirplanesData.MainAirplane(_mainAirport.Id);
    }

    [Fact]
    public async Task ShouldCreateAirplane()
    {
        // Arrange
        var request = new CreateAirplaneDto(
            Model: "Boeing 737",
            MaxPassenger: 180,
            YearOfManufacture: 2000,
            AirportId: _mainAirport.Id.Value);

        // Act
        var response = await Client.PostAsJsonAsync("airplanes/create", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseAirplane = await response.Content.ReadFromJsonAsync<CreateAirplaneDto>();
        responseAirplane.Should().NotBeNull();
        responseAirplane!.Model.Should().Be("Boeing 737");
        responseAirplane.MaxPassenger.Should().Be(180);
        
        var createdAirplane = await Context.Airplanes
            .FirstOrDefaultAsync(a => a.Model == "Boeing 737" && a.MaxPassenger == 180);
        createdAirplane.Should().NotBeNull();
    }

    [Fact]
    public async Task ShouldGetAllAirplanes()
    {
        // Act
        var response = await Client.GetAsync("airplanes/GetAll");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var airplanes = await response.Content.ReadFromJsonAsync<IReadOnlyList<AirplaneDto>>();
        airplanes.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task ShouldGetAirplaneById()
    {
        // Act
        var response = await Client.GetAsync($"airplanes/{_mainAirplane.Id.Value}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var airplane = await response.Content.ReadFromJsonAsync<AirplaneDto>();
        airplane.Should().NotBeNull();
        airplane!.Id.Should().Be(_mainAirplane.Id.Value);
    }

    [Fact]
    public async Task GetAirplaneById_ShouldReturnNotFound_WhenIdIsInvalid()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync($"airplanes/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldUpdateAirplane()
    {
        // Arrange
        var updateRequest = new UpdateAirplaneDto(
            AirplaneId: _mainAirplane.Id.Value,
            Model: "Airbus A320",
            MaxPassenger: 200,
            YearOfManufacture: 2015,
            AirportId: _mainAirport.Id.Value);

        // Act
        var response = await Client.PutAsJsonAsync("airplanes/Update", updateRequest);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var updatedAirplane = await response.Content.ReadFromJsonAsync<AirplaneDto>();
        updatedAirplane.Should().NotBeNull();
        updatedAirplane!.Model.Should().Be(updateRequest.Model);
        updatedAirplane.MaxPassenger.Should().Be(updateRequest.MaxPassenger);
        
        var dbAirplane = await Context.Airplanes.FirstOrDefaultAsync(a => a.Id == _mainAirplane.Id);
        dbAirplane.Should().NotBeNull();
        dbAirplane!.Model.Should().Be("Airbus A320");
    }

    [Fact]
    public async Task UpdateAirplane_ShouldReturnNotFound_WhenIdIsInvalid()
    {
        // Arrange
        var invalidId = Guid.NewGuid();
        var updateRequest = new UpdateAirplaneDto(
            AirplaneId: invalidId,
            Model: "Nonexistent Model",
            MaxPassenger: 100,
            YearOfManufacture: 1999,
            AirportId: Guid.NewGuid());

        // Act
        var response = await Client.PutAsJsonAsync("airplanes/Update", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldDeleteAirplane()
    {
        // Act
        var response = await Client.DeleteAsync($"airplanes/{_mainAirplane.Id.Value}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var deletedAirplane = await Context.Airplanes.FindAsync(_mainAirplane.Id);
        deletedAirplane.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAirplane_ShouldReturnNotFound_WhenIdIsInvalid()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act
        var response = await Client.DeleteAsync($"airplanes/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateAirplane_ShouldReturnError_WhenDataIsInvalid()
    {
        // Arrange
        var request = new CreateAirplaneDto(
            Model: "", 
            MaxPassenger: -1, 
            YearOfManufacture: 0, 
            AirportId: Guid.NewGuid());

        // Act
        var response = await Client.PostAsJsonAsync("airplanes/create", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
    }
    
    [Fact]
    public async Task CreateAirplane_ShouldReturnBadRequest_WhenModelIsMissing()
    {
        // Arrange
        var request = new CreateAirplaneDto(
            Model: "",
            MaxPassenger: 150,
            YearOfManufacture: 2020,
            AirportId: _mainAirport.Id.Value);

        // Act
        var response = await Client.PostAsJsonAsync("airplanes/create", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

    }





    public async Task InitializeAsync()
    {
        await Context.Airports.AddAsync(_mainAirport);
        await Context.Airplanes.AddAsync(_mainAirplane);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Airplanes.RemoveRange(Context.Airplanes);
        Context.Airports.RemoveRange(Context.Airports);
        await SaveChangesAsync();
    }
}
