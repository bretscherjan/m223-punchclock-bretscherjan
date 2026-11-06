using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using M223PunchclockDotnet.Model;

namespace M223PunchclockDotnet.Service
{
    public class ExampleDataService
    {
        private readonly DatabaseContext _databaseContext;

        public ExampleDataService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task CreateExampleDataAsync()
        {
            if (await _databaseContext.Categories.AnyAsync()
                || await _databaseContext.Tags.AnyAsync()
                || await _databaseContext.Entries.AnyAsync())
            {
                return;
            }

            var today = DateTime.UtcNow.Date;

            DateTime At(int daysAgo, int hour) =>
                new DateTime(today.Year, today.Month, today.Day, hour, 0, 0, DateTimeKind.Utc).AddDays(-daysAgo);

            var work = new Category { Title = "Arbeit" };
            var school = new Category { Title = "Schule" };
            var personal = new Category { Title = "Privat" };

            var meeting = new Tag { Title = "Meeting" };
            var coding = new Tag { Title = "Programmieren" };
            var learning = new Tag { Title = "Lernen" };
            var review = new Tag { Title = "Review" };

            var entry1 = new Entry { CheckIn = At(3, 8), CheckOut = At(3, 12), Category = work };
            entry1.Tags.Add(meeting);
            entry1.Tags.Add(coding);

            var entry2 = new Entry { CheckIn = At(2, 13), CheckOut = At(2, 17), Category = work };
            entry2.Tags.Add(review);

            var entry3 = new Entry { CheckIn = At(1, 8), CheckOut = At(1, 11), Category = school };
            entry3.Tags.Add(learning);

            var entry4 = new Entry { CheckIn = At(0, 18), CheckOut = At(0, 20), Category = personal };
            entry4.Tags.Add(learning);
            entry4.Tags.Add(coding);

            _databaseContext.Categories.AddRange(work, school, personal);
            _databaseContext.Tags.AddRange(meeting, coding, learning, review);
            _databaseContext.Entries.AddRange(entry1, entry2, entry3, entry4);

            await _databaseContext.SaveChangesAsync();
        }
    }
}
