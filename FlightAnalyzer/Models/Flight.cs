using System;

namespace FlightAnalyzer.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public string? AircraftRegistrationNumber { get; set; }
        public string? AircraftType { get; set; }
        public string? FlightNumber { get; set; }
        public string? DepartureAirport { get; set; }
        public DateTime? DepartureDateTime { get; set; }
        public string? ArrivalAirport { get; set; }
        public DateTime? ArrivalDateTime { get; set; }
    }
}
