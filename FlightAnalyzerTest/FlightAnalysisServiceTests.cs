using FlightAnalyzer.Models;
using FlightAnalyzer.Services;

namespace FlightAnalyzerTest
{
    public class FlightAnalysisServiceTests
    {
        [Fact]
        public void AnalyzeFlightChains_FlightsWithConsistentSequence_ReturnsEmptyList()
        {
            // Arrange
            var flights = new List<Flight>
            {
                new Flight { AircraftRegistrationNumber = "ABC-123", FlightNumber = "M645", DepartureAirport = "HEL", ArrivalAirport = "DXB", DepartureDateTime = DateTime.Parse("2024-01-02 21:46:27"), ArrivalDateTime = DateTime.Parse("2024-01-03 02:31:27") },
                new Flight { AircraftRegistrationNumber = "ABC-123", FlightNumber = "K319", DepartureAirport = "DXB", ArrivalAirport = "SFO", DepartureDateTime = DateTime.Parse("2024-01-03 04:00:00"), ArrivalDateTime = DateTime.Parse("2024-01-03 08:00:00") }
            };
            var service = new FlightAnalysisService();

            // Act
            var result = service.AnalyzeFlightChains(flights);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void AnalyzeFlightChains_FlightsWithInconsistentSequence_ReturnsInconsistencies()
        {
            // Arrange
            var flights = new List<Flight>
            {
                new Flight { AircraftRegistrationNumber = "ABC-123", FlightNumber = "M645", DepartureAirport = "HEL", ArrivalAirport = "DXB", DepartureDateTime = DateTime.Parse("2024-01-02 21:46:27"), ArrivalDateTime = DateTime.Parse("2024-01-03 02:31:27") },
                new Flight { AircraftRegistrationNumber = "ABC-123", FlightNumber = "K319", DepartureAirport = "SFO", ArrivalAirport = "DXB", DepartureDateTime = DateTime.Parse("2024-01-03 02:00:00"), ArrivalDateTime = DateTime.Parse("2024-01-03 08:00:00") } // Inconsistent with M645's arrival
            };
            var service = new FlightAnalysisService();

            // Act
            var result = service.AnalyzeFlightChains(flights);

            // Assert
            Assert.Equal(2, result.Count);
            // Further assertions can be made to verify the contents of the inconsistencies
        }
    }
}
