using System.Net;
using System.Net.Http.Json;
using Api.Dtos.Airports;
using Domain.Airports;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data;
using Xunit;

namespace Api.Tests.Integration.Airports
{
    public class AirportsControllerTests : BaseIntegrationTest, IAsyncLifetime
    {
        private readonly Airport _mainAirport;

        public AirportsControllerTests(IntegrationTestWebFactory factory) : base(factory)
        {
            _mainAirport = AirportsData.MainAirport();
        }

        [Fact]
        public async Task ShouldCreateAirport()
        {
            // Arrange
            var request = new CreateAirportDto(
                Name: "Test Airport",
                Location: "Test Location");

            // Act
            var response = await Client.PostAsJsonAsync("airports/create", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK); 

            var responseAirport = await response.Content.ReadFromJsonAsync<CreateAirportDto>();
            responseAirport.Should().NotBeNull();
            responseAirport!.Name.Should().Be("Test Airport");
            responseAirport.Location.Should().Be("Test Location");
            
            var createdAirport = await Context.Airports
                .FirstOrDefaultAsync(a => a.Name == "Test Airport" && a.Location == "Test Location");
            createdAirport.Should().NotBeNull();
        }


        [Fact]
        public async Task ShouldGetAllAirports()
        {
            // Act
            var response = await Client.GetAsync("airports/getAll");

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            var airports = await response.Content.ReadFromJsonAsync<IReadOnlyList<AirportDto>>();
            airports.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ShouldGetAirportById()
        {
            // Act
            var response = await Client.GetAsync($"airports/{_mainAirport.Id.Value}");

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            var airport = await response.Content.ReadFromJsonAsync<AirportDto>();
            airport.Should().NotBeNull();
            airport!.Id.Should().Be(_mainAirport.Id.Value);
        }

        [Fact]
        public async Task GetAirportById_ShouldReturnNotFound_WhenIdIsInvalid()
        {
            // Arrange
            var invalidId = Guid.NewGuid();

            // Act
            var response = await Client.GetAsync($"airports/{invalidId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ShouldUpdateAirport()
        {
            // Arrange
            var updateRequest = new AirportDto(
                Id: _mainAirport.Id.Value,
                Name: "Updated Airport",
                Location: "Updated Location");

            // Act
            var response = await Client.PutAsJsonAsync($"airports/{_mainAirport.Id.Value}", updateRequest);

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            var updatedAirport = await response.Content.ReadFromJsonAsync<AirportDto>();
            updatedAirport.Should().NotBeNull();
            updatedAirport!.Name.Should().Be(updateRequest.Name);
            updatedAirport.Location.Should().Be(updateRequest.Location);
            
            var dbAirport = await Context.Airports
                .FirstOrDefaultAsync(a => a.Id == _mainAirport.Id);
            dbAirport.Should().NotBeNull();
            dbAirport!.Name.Should().Be("Updated Airport");
            dbAirport.Location.Should().Be("Updated Location");
        }

        [Fact]
        public async Task UpdateAirport_ShouldReturnNotFound_WhenIdIsInvalid()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            var updateRequest = new AirportDto(
                Id: invalidId,
                Name: "Nonexistent Airport",
                Location: "Nonexistent Location");

            // Act
            var response = await Client.PutAsJsonAsync($"airports/{invalidId}", updateRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ShouldDeleteAirport()
        {
            // Act
            var response = await Client.DeleteAsync($"airports/{_mainAirport.Id.Value}");

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            var deletedAirport = await Context.Airports.FindAsync(_mainAirport.Id);
            deletedAirport.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAirport_ShouldReturnNotFound_WhenIdIsInvalid()
        {
            // Arrange
            var invalidId = Guid.NewGuid();

            // Act
            var response = await Client.DeleteAsync($"airports/{invalidId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateAirport_ShouldReturnError_WhenDataIsInvalid()
        {
            // Arrange
            var request = new CreateAirportDto(
                Name: "", 
                Location: "Location");

            // Act
            var response = await Client.PostAsJsonAsync("airports/create", request);

            // Assert
            response.IsSuccessStatusCode.Should().BeFalse(); 
        }



        [Fact]
        public async Task UpdateAirport_ShouldReturnBadRequest_WhenDataIsInvalid()
        {
            // Arrange
            var updateRequest = new AirportDto(
                Id: _mainAirport.Id.Value,
                Name: "",
                Location: "Updated Location");

            // Act
            var response = await Client.PutAsJsonAsync($"airports/{_mainAirport.Id.Value}", updateRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public async Task InitializeAsync()
        {
            await Context.Airports.AddAsync(_mainAirport);
            await SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            Context.Airports.RemoveRange(Context.Airports);
            await SaveChangesAsync();
        }
    }
}
