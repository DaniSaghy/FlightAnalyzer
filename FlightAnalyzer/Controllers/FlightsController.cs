using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using FlightAnalyzer.Models;
using FlightAnalyzer.Services;

namespace FlightAnalyzer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightService _flightService;
        private readonly IFlightAnalysisService _flightAnalysisService;

        public FlightsController(IFlightService flightService, IFlightAnalysisService flightAnalysisService)
        {
            _flightService = flightService;
            _flightAnalysisService = flightAnalysisService;
        }

        /// <summary>
        /// Gets a list of all flights.
        /// </summary>
        /// <returns>A list of flights.</returns>
        /// <response code="200">Returns the list of flights</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Flight>> GetFlights()
        {
            var flights = _flightService.GetAllFlights();
            return Ok(flights);
        }

        /// <summary>
        /// Analyzes flights for scheduling inconsistencies.
        /// </summary>
        /// <returns>A list of flights with inconsistencies, or no content if all flights are consistent.</returns>
        /// <response code="200">Returns the list of inconsistent flights</response>
        /// <response code="204">No inconsistencies found</response>
        [HttpGet("analyze")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<FlightInconsistency>> AnalyzeFlights()
        {
            var flights = _flightService.GetAllFlights();
            var inconsistencies = _flightAnalysisService.AnalyzeFlightChains(flights);
            if (inconsistencies == null || inconsistencies.Count == 0)
            {
                return NoContent();
            }
            return Ok(inconsistencies);
        }
    }
}
