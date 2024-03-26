using Moq;
using Microsoft.Extensions.Logging;
using System.IO.Abstractions.TestingHelpers;
using FlightAnalyzer.Services;

namespace FlightAnalyzerTest
{
    public class FlightServiceTests
    {
        private readonly Mock<ILogger<FlightService>> _mockLogger;
        private readonly MockFileSystem _mockFileSystem;
        private readonly string _csvFilePath = "./Data/flights.csv";

        public FlightServiceTests()
        {
            _mockLogger = new Mock<ILogger<FlightService>>();
            _mockFileSystem = new MockFileSystem();
            // Prepare the mock file system with a sample CSV content
            var mockInputFile = new MockFileData(
                "id,aircraft_registration_number,aircraft_type,flight_number,departure_airport,departure_datetime,arrival_airport,arrival_datetime\n" +
                "1,ZX-IKD,350,M645,HEL,2024-01-02 21:46:27,DXB,2024-01-03 02:31:27");
            _mockFileSystem.AddFile(_csvFilePath, mockInputFile);
        }

        [Fact]
        public void GetAllFlights_FileExists_ReturnsFlights()
        {
            // Arrange
            var service = new FlightService(_mockLogger.Object, _mockFileSystem, _csvFilePath);

            // Act
            var result = service.GetAllFlights();

            // Assert
            Assert.NotEmpty(result);
            var flight = result.First();
            Assert.Equal("ZX-IKD", flight.AircraftRegistrationNumber);
        }

        [Fact]
        public void GetAllFlights_FileNotFound_LogsError()
        {
            // Arrange
            var nonExistentPath = "./Data/nonexistent.csv";
            var service = new FlightService(_mockLogger.Object, _mockFileSystem, nonExistentPath);

            // Act
            var result = service.GetAllFlights();

            // Assert
            Assert.Empty(result);

            _mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("The file was not found")),
                    It.IsAny<FileNotFoundException>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

        }
    }
}