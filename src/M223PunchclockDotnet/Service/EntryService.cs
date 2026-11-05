using M223PunchclockDotnet.Model;
using Microsoft.EntityFrameworkCore;

namespace M223PunchclockDotnet.Service
{
    public class EntryService
    {
        private DatabaseContext _databaseContext; 

        public EntryService(DatabaseContext databaseContext){
            _databaseContext = databaseContext;
        }
        public Task<List<Entry>> FindAll()
        {
            return _databaseContext.Entries.ToListAsync();
        }

        public async Task<Entry> AddEntry(Entry entry)
        {
            _databaseContext.Entries.Add(entry);
            await _databaseContext.SaveChangesAsync();

            return entry;
        }

        public async Task<Entry> DeleteById(int id)
        {
            var entry = await _databaseContext.Entries.FindAsync(id);
            if (entry != null)
            {
                _databaseContext.Entries.Remove(entry);
                await _databaseContext.SaveChangesAsync();
            }
            return entry;
        }

        public async Task<Entry> FindById(int id)
        {
            var entry = await _databaseContext.Entries.FindAsync(id);
            return entry;
        }

        public async Task<Entry> UpdateEntry(Entry entry)
        {
            var existingEntry = await _databaseContext.Entries.FindAsync(entry.Id);
            if (existingEntry != null)
            {
                await _databaseContext.SaveChangesAsync();
            }
            return existingEntry;
        }
    }
}
