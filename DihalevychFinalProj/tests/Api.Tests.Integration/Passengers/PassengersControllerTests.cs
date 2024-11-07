using System.Net;
using System.Net.Http.Json;
using Api.Dtos.Passengers;
using Domain.Passengers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data;
using Xunit;

namespace Api.Tests.Integration.Passengers
{
    public class PassengersControllerTests : BaseIntegrationTest, IAsyncLifetime
    {
        private readonly Passenger _mainPassenger;

        public PassengersControllerTests(IntegrationTestWebFactory factory) : base(factory)
        {
            _mainPassenger = PassengersData.MainPassenger();
        }

        [Fact]
        public async Task ShouldCreatePassenger()
        {
            // Arrange
            var request = new CreatePassengerDto(
                FirstName: "Alice",
                LastName: "Smith",
                Age: 28
            );

            // Act
            var response = await Client.PostAsJsonAsync("passengers", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responsePassenger = await response.Content.ReadFromJsonAsync<PassengerDto>();
            responsePassenger.Should().NotBeNull();
            responsePassenger!.FirstName.Should().Be("Alice");
            responsePassenger.LastName.Should().Be("Smith");
            
            var createdPassenger = await Context.Passengers
                .FirstOrDefaultAsync(p => p.FirstName == "Alice" && p.LastName == "Smith");
            createdPassenger.Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldGetAllPassengers()
        {
            // Act
            var response = await Client.GetAsync("passengers");

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            var passengers = await response.Content.ReadFromJsonAsync<IReadOnlyList<PassengerDto>>();
            passengers.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ShouldGetPassengerById()
        {
            // Act
            var response = await Client.GetAsync($"passengers/{_mainPassenger.Id.Value}");

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            var passenger = await response.Content.ReadFromJsonAsync<PassengerDto>();
            passenger.Should().NotBeNull();
            passenger!.Id.Should().Be(_mainPassenger.Id.Value);
        }

        [Fact]
        public async Task GetPassengerById_ShouldReturnNotFound_WhenIdIsInvalid()
        {
            // Arrange
            var invalidId = Guid.NewGuid();

            // Act
            var response = await Client.GetAsync($"passengers/{invalidId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ShouldUpdatePassenger()
        {
            // Arrange
            var updateRequest = new PassengerDto(
                Id: _mainPassenger.Id.Value,
                FirstName: "John",
                LastName: "Doe Updated",
                Age: 31
            );

            // Act
            var response = await Client.PutAsJsonAsync($"passengers/{_mainPassenger.Id.Value}", updateRequest);

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            var updatedPassenger = await response.Content.ReadFromJsonAsync<PassengerDto>();
            updatedPassenger.Should().NotBeNull();
            updatedPassenger!.FirstName.Should().Be("John");
            updatedPassenger.LastName.Should().Be("Doe Updated");
            
            var dbPassenger = await Context.Passengers.FirstOrDefaultAsync(p => p.Id == _mainPassenger.Id);
            dbPassenger.Should().NotBeNull();
            dbPassenger!.LastName.Should().Be("Doe Updated");
        }

        [Fact]
        public async Task UpdatePassenger_ShouldReturnNotFound_WhenIdIsInvalid()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            var updateRequest = new PassengerDto(
                Id: invalidId,
                FirstName: "Nonexistent",
                LastName: "Passenger",
                Age: 25
            );

            // Act
            var response = await Client.PutAsJsonAsync($"passengers/{invalidId}", updateRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ShouldDeletePassenger()
        {
            // Act
            var response = await Client.DeleteAsync($"passengers/{_mainPassenger.Id.Value}");

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            var deletedPassenger = await Context.Passengers.FindAsync(_mainPassenger.Id);
            deletedPassenger.Should().BeNull();
        }

        [Fact]
        public async Task DeletePassenger_ShouldReturnNotFound_WhenIdIsInvalid()
        {
            // Arrange
            var invalidId = Guid.NewGuid();

            // Act
            var response = await Client.DeleteAsync($"passengers/{invalidId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreatePassenger_ShouldReturnError_WhenDataIsInvalid()
        {
            // Arrange
            var request = new CreatePassengerDto(
                FirstName: "", 
                LastName: "", 
                Age: -1
            );

            // Act
            var response = await Client.PostAsJsonAsync("passengers", request);

            // Assert
            response.IsSuccessStatusCode.Should().BeFalse();
        }

        [Fact]
        public async Task UpdatePassenger_ShouldReturnBadRequest_WhenInvalidData()
        {
            // Arrange
            var invalidRequest = new PassengerDto(
                Id: _mainPassenger.Id.Value,
                FirstName: "", 
                LastName: "", 
                Age: -1
            );

            // Act
            var response = await Client.PutAsJsonAsync($"passengers/{_mainPassenger.Id.Value}", invalidRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public async Task InitializeAsync()
        {
            await Context.Passengers.AddAsync(_mainPassenger);
            await SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            Context.Passengers.RemoveRange(Context.Passengers);
            await SaveChangesAsync();
        }
    }
}
