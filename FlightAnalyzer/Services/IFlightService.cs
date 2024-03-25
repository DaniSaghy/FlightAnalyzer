using FlightAnalyzer.Models;

namespace FlightAnalyzer.Services
{
    public interface IFlightService
    {
        IEnumerable<Flight> GetAllFlights();
    }
}
