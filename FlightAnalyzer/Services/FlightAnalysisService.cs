﻿using FlightAnalyzer.Models;

namespace FlightAnalyzer.Services
{
    public class FlightAnalysisService : IFlightAnalysisService
    {
        public List<FlightInconsistency> AnalyzeFlightChains(IEnumerable<Flight> flights)
        {
            var inconsistencies = new List<FlightInconsistency>();

            // Map departure airports to flights
            var flightsByAircraft = flights
                .GroupBy(f => string.IsNullOrEmpty(f.AircraftRegistrationNumber) ? "Unknown" : f.AircraftRegistrationNumber)
                .ToDictionary(g => g.Key, g => g.OrderBy(f => f.DepartureDateTime).ToList());


            var flightChains = flightsByAircraft.Where(x => x.Value.Count > 1);

            foreach (var flight in flightChains)
            {
                for (int i = 0; i < flight.Value.Count - 1; i++)
                {
                    var currentFlight = flight.Value[i];
                    var nextFlight = flight.Value[i + 1];

                    // Check for mismatch in airport sequence
                    if (currentFlight.ArrivalAirport != nextFlight.DepartureAirport)
                    {
                        inconsistencies.Add(new FlightInconsistency
                        {
                            AircraftRegistrationNumber = currentFlight.AircraftRegistrationNumber,
                            CurrentFlight = currentFlight,
                            NextFlight = nextFlight,
                            IssueDescription = $"Mismatch in airport sequence. Expected next departure from {currentFlight.ArrivalAirport}, but got {nextFlight.DepartureAirport}."

                        });
                    }

                    // Check for overlapping flight times
                    if (currentFlight.ArrivalDateTime > nextFlight.DepartureDateTime)
                    {
                        inconsistencies.Add(new FlightInconsistency
                        {
                            AircraftRegistrationNumber = currentFlight.AircraftRegistrationNumber,
                            CurrentFlight = currentFlight,
                            NextFlight = nextFlight,
                            IssueDescription = $"Timing overlap detected. Current flight arrives at {currentFlight.ArrivalDateTime}, but next flight departs at {nextFlight.DepartureDateTime}."
                        });
                    }
                }
            }

            return inconsistencies;
        }
    }
}
