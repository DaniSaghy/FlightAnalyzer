using CsvHelper;
using CsvHelper.Configuration;
using FlightAnalyzer.Models;
using System.Globalization;

namespace FlightAnalyzer.Services
{
    public class FlightService : IFlightService
    {
        private readonly ILogger<FlightService> _logger;
        private readonly string _csvFilePath = "./Data/flights.csv"; // Update this path

        public FlightService(ILogger<FlightService> logger)
        {
            _logger = logger;
        }

        public IEnumerable<Flight> GetAllFlights()
        {
            try
            {
                using var reader = new StreamReader(_csvFilePath);
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));
                csv.Context.RegisterClassMap<FlightMap>();
                var flights = csv.GetRecords<Flight>().ToList();
                return flights;
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError($"The file was not found: '{ex.Message}'");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: '{ex.Message}'");
            }
            return [];
        }
    }
}
