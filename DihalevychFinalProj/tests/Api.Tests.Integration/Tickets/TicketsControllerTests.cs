using System.Net;
using System.Net.Http.Json;
using Api.Dtos.Tickets;
using Domain.Airplanes;
using Domain.Airports;
using Domain.Flights;
using Domain.Passengers;
using Domain.Tickets;
using FluentAssertions;
using Tests.Common;
using Tests.Data;
using Xunit;

namespace Api.Tests.Integration.Tickets
{
    public class TicketsControllerTests : BaseIntegrationTest, IAsyncLifetime
    {
        private readonly Airport _departureAirport;
        private readonly Airport _arrivalAirport;
        private readonly Airplane _airplane;
        private readonly Flight _flight;
        private readonly Passenger _passenger;
        private readonly Ticket _mainTicket;

        public TicketsControllerTests(IntegrationTestWebFactory factory) : base(factory)
        {
            // Ініціалізація базових даних для тестів
            _departureAirport = AirportsData.MainAirport();
            _arrivalAirport = AirportsData.MainAirport();
            _airplane = AirplanesData.MainAirplane(_departureAirport.Id);
            _flight = FlightsData.MainFlight(_departureAirport.Id, _arrivalAirport.Id, _airplane.Id);
            _passenger = PassengersData.MainPassenger();
            _mainTicket = TicketsData.MainTicket(_flight.Id, _passenger.Id);
        }

        [Fact]
        public async Task ShouldCreateTicket()
        {
            // Arrange
            var request = new CreateTicketDto(
                FlightId: _flight.Id.Value,
                PassengerId: _passenger.Id.Value);

            // Act
            var response = await Client.PostAsJsonAsync("tickets", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseTicket = await response.Content.ReadFromJsonAsync<CreateTicketDto>();
            responseTicket.Should().NotBeNull();
            responseTicket!.FlightId.Should().Be(_flight.Id.Value);
            responseTicket.PassengerId.Should().Be(_passenger.Id.Value);
        }

        [Fact]
        public async Task ShouldGetAllTickets()
        {
            // Act
            var response = await Client.GetAsync("tickets");

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            var tickets = await response.Content.ReadFromJsonAsync<IReadOnlyList<TicketDto>>();
            tickets.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ShouldGetTicketById()
        {
            // Act
            var response = await Client.GetAsync($"tickets/{_mainTicket.Id.Value}");

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            var ticket = await response.Content.ReadFromJsonAsync<TicketDto>();
            ticket.Should().NotBeNull();
            ticket!.Id.Should().Be(_mainTicket.Id.Value);
        }

        [Fact]
        public async Task GetTicketById_ShouldReturnNotFound_WhenIdIsInvalid()
        {
            // Arrange
            var invalidId = Guid.NewGuid();

            // Act
            var response = await Client.GetAsync($"tickets/{invalidId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ShouldUpdateTicket()
        {
            // Arrange
            var updateRequest = new UpdateTicketDto(
                FlightId: _flight.Id.Value,
                PassengerId: _passenger.Id.Value);

            // Act
            var response = await Client.PutAsJsonAsync($"tickets/{_mainTicket.Id.Value}", updateRequest);

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            var updatedTicket = await response.Content.ReadFromJsonAsync<TicketDto>();
            updatedTicket.Should().NotBeNull();
            updatedTicket!.FlightId.Should().Be(updateRequest.FlightId);
            updatedTicket.PassengerId.Should().Be(updateRequest.PassengerId);
        }

        [Fact]
        public async Task UpdateTicket_ShouldReturnNotFound_WhenIdIsInvalid()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            var updateRequest = new UpdateTicketDto(
                FlightId: _flight.Id.Value,
                PassengerId: _passenger.Id.Value);

            // Act
            var response = await Client.PutAsJsonAsync($"tickets/{invalidId}", updateRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ShouldDeleteTicket()
        {
            // Act
            var response = await Client.DeleteAsync($"tickets/{_mainTicket.Id.Value}");

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            var deletedTicket = await Context.Tickets.FindAsync(_mainTicket.Id);
            deletedTicket.Should().BeNull();
        }

        [Fact]
        public async Task DeleteTicket_ShouldReturnNotFound_WhenIdIsInvalid()
        {
            // Arrange
            var invalidId = Guid.NewGuid();

            // Act
            var response = await Client.DeleteAsync($"tickets/{invalidId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateTicket_ShouldReturnError_WhenDataIsInvalid()
        {
            // Arrange
            var request = new CreateTicketDto(
                FlightId: Guid.Empty,
                PassengerId: Guid.Empty);

            // Act
            var response = await Client.PostAsJsonAsync("tickets", request);

            // Assert
            response.IsSuccessStatusCode.Should().BeFalse();
        }

        [Fact]
        public async Task CreateTicket_ShouldReturnBadRequest_WhenFlightIdIsMissing()
        {
            // Arrange
            var request = new CreateTicketDto(
                FlightId: Guid.Empty,
                PassengerId: _passenger.Id.Value);

            // Act
            var response = await Client.PostAsJsonAsync("tickets", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        public async Task InitializeAsync()
        {
            // Додаємо всі об'єкти в контекст
            await Context.Airports.AddRangeAsync(_departureAirport, _arrivalAirport);
            await Context.Airplanes.AddAsync(_airplane);
            await Context.Flights.AddAsync(_flight);
            await Context.Passengers.AddAsync(_passenger);
            await Context.Tickets.AddAsync(_mainTicket);
            await SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            // Видаляємо всі об'єкти після виконання тестів
            Context.Tickets.RemoveRange(Context.Tickets);
            Context.Flights.RemoveRange(Context.Flights);
            Context.Airplanes.RemoveRange(Context.Airplanes);
            Context.Passengers.RemoveRange(Context.Passengers);
            Context.Airports.RemoveRange(Context.Airports);
            await SaveChangesAsync();
        }
    }
}
