using CsvHelper;
using CsvHelper.Configuration;
using FlightAnalyzer.Models;
using System.Globalization;
using System.IO.Abstractions;

namespace FlightAnalyzer.Services
{
    public class FlightService : IFlightService
    {
        private readonly ILogger<FlightService> _logger;
        private readonly IFileSystem _fileSystem;
        private readonly string _csvFilePath;

        public FlightService(ILogger<FlightService> logger, IFileSystem fileSystem, string csvFilePath)
        {
            _logger = logger;
            _fileSystem = fileSystem;
            _csvFilePath = csvFilePath;
        }

        public IEnumerable<Flight> GetAllFlights()
        {
            try
            {
                using var reader = _fileSystem.File.OpenText(_csvFilePath);
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
