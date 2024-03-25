using FlightAnalyzer.Models;

namespace FlightAnalyzer.Services
{
    public interface IFlightAnalysisService
    {
        List<FlightInconsistency> AnalyzeFlightChains(IEnumerable<Flight> flights);
    }
}
