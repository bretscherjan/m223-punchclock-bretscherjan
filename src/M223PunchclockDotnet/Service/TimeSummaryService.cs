using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using M223PunchclockDotnet.Model;

namespace M223PunchclockDotnet.Service
{
    public record DailySummary(DateOnly Date, TimeSpan TotalDuration);
    public class TimeSummaryService
    {

        private readonly DatabaseContext _databaseContext;

        public TimeSummaryService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }


        public async Task<List<DailySummary>> GetSummaryPerDayAsync()
        {
            var entries = await _databaseContext.Entries.ToListAsync();
            return CalculateSummaryPerDay(entries);
        }

        private List<DailySummary> CalculateSummaryPerDay(List<Entry> entries)
        {
            return entries
                .GroupBy(e => DateOnly.FromDateTime(e.CheckIn))
                .Select(g => new DailySummary(
                    g.Key,
                    g.Aggregate(TimeSpan.Zero, (total, entry) => total + (entry.CheckOut - entry.CheckIn))
                ))
                .ToList();
        }
    }
}