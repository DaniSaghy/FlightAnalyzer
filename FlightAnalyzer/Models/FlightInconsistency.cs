namespace FlightAnalyzer.Models
{
    public class FlightInconsistency
    {
        public string IssueDescription { get; set; }
        public string AircraftRegistrationNumber { get; set; }
        public Flight CurrentFlight { get; set; }
        public Flight NextFlight { get; set; }

        public FlightInconsistency(
            string issueDescription,
            string aircraftRegistrationNumber, 
            Flight currentFlight, 
            Flight nextFlight)
        {
            IssueDescription = issueDescription;
            AircraftRegistrationNumber = aircraftRegistrationNumber;
            CurrentFlight = currentFlight;
            NextFlight = nextFlight;            
        }
    }
}
