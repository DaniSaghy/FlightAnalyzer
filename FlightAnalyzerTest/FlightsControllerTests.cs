using FlightAnalyzer.Controllers;
using FlightAnalyzer.Models;
using FlightAnalyzer.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FlightAnalyzerTest
{
    public class FlightsControllerTests
    {
        private readonly Mock<IFlightService> _mockFlightService = new Mock<IFlightService>();
        private readonly Mock<IFlightAnalysisService> _mockFlightAnalysisService = new Mock<IFlightAnalysisService>();
        private readonly FlightsController _controller;

        public FlightsControllerTests()
        {
            _controller = new FlightsController(_mockFlightService.Object, _mockFlightAnalysisService.Object);
        }

        [Fact]
        public void GetFlights_ReturnsAllFlights()
        {
            // Arrange
            var mockFlights = new List<Flight> { new Flight { AircraftRegistrationNumber = "ABC-123", FlightNumber = "M645", DepartureAirport = "HEL", ArrivalAirport = "DXB", DepartureDateTime = DateTime.Parse("2024-01-02 21:46:27"), ArrivalDateTime = DateTime.Parse("2024-01-03 02:31:27") } };
            _mockFlightService.Setup(service => service.GetAllFlights()).Returns(mockFlights);

            // Act
            var result = _controller.GetFlights();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedFlights = Assert.IsType<List<Flight>>(actionResult.Value);
            Assert.Single(returnedFlights);
            Assert.Equal("ABC-123", returnedFlights.First().AircraftRegistrationNumber);
        }

        [Fact]
        public void AnalyzeFlights_FoundInconsistencies_ReturnsInconsistencies()
        {
            // Arrange
            var mockInconsistencies = new List<FlightInconsistency> { new FlightInconsistency { IssueDescription = "Test issue", AircraftRegistrationNumber = "ABC-123", CurrentFlight = new(), NextFlight = new() } };
            _mockFlightService.Setup(service => service.GetAllFlights()).Returns(new List<Flight>());
            _mockFlightAnalysisService.Setup(service => service.AnalyzeFlightChains(It.IsAny<IEnumerable<Flight>>())).Returns(mockInconsistencies);

            // Act
            var result = _controller.AnalyzeFlights();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedInconsistencies = Assert.IsType<List<FlightInconsistency>>(actionResult.Value);
            Assert.Single(returnedInconsistencies);
            Assert.Equal("Test issue", returnedInconsistencies.First().IssueDescription);
        }

        [Fact]
        public void AnalyzeFlights_NoInconsistencies_ReturnsNoContent()
        {
            // Arrange
            _mockFlightService.Setup(service => service.GetAllFlights()).Returns(new List<Flight>());
            _mockFlightAnalysisService.Setup(service => service.AnalyzeFlightChains(It.IsAny<IEnumerable<Flight>>())).Returns(new List<FlightInconsistency>());

            // Act
            var result = _controller.AnalyzeFlights();

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }
    }
}
