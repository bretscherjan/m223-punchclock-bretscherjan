using System.Net.Mime;
using M223PunchclockDotnet.Dto;
using M223PunchclockDotnet.Model;
using M223PunchclockDotnet.Service;
using Microsoft.AspNetCore.Mvc;

namespace M223PunchclockDotnet.Controllers
{
    [ApiController]
    [Route("statistics/time-summaries")]
    public class TimeSummaryController : ControllerBase
    {
        private readonly TimeSummaryService _timeSummaryService;

        public TimeSummaryController(TimeSummaryService timeSummaryService)
        {
            _timeSummaryService = timeSummaryService;
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<List<DailySummary>>> GetSummaryPerDayAsync()
        {
            var summary = await _timeSummaryService.GetSummaryPerDayAsync();
            return Ok(summary);
        }

    }
}