using M223PunchclockDotnet.Model;
using Microsoft.EntityFrameworkCore;

namespace M223PunchclockDotnet.Service
{
    /// <summary>Loaded category and tags for a request. Missing lists the IDs that do not exist.</summary>
    public sealed record RelationLookup(Category? Category, List<Tag> Tags, List<string> Missing);

    public class EntryService
    {
        private readonly DatabaseContext _databaseContext;

        public EntryService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Task<List<Entry>> FindAll()
        {
            return _databaseContext.Entries
                .Include(e => e.Category)
                .Include(e => e.Tags)
                .ToListAsync();
        }

        public async Task<Entry?> FindById(int id)
        {
            return await _databaseContext.Entries
                .Include(e => e.Category)
                .Include(e => e.Tags)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<RelationLookup> LookupRelations(int? categoryId, IEnumerable<int> tagIds)
        {
            var missing = new List<string>();

            Category? category = null;
            if (categoryId is int cid)
            {
                category = await _databaseContext.Categories.FindAsync(cid);
                if (category == null)
                {
                    missing.Add($"category {cid}");
                }
            }

            var ids = tagIds.Distinct().ToList();
            var tags = ids.Count == 0
                ? new List<Tag>()
                : await _databaseContext.Tags.Where(t => ids.Contains(t.Id)).ToListAsync();
            missing.AddRange(ids.Except(tags.Select(t => t.Id)).Select(id => $"tag {id}"));

            return new RelationLookup(category, tags, missing);
        }

        public async Task<Entry> AddEntry(Entry entry)
        {
            _databaseContext.Entries.Add(entry);
            await _databaseContext.SaveChangesAsync();

            return entry;
        }

        public async Task<Entry?> DeleteById(int id)
        {
            var entry = await _databaseContext.Entries.FindAsync(id);
            if (entry != null)
            {
                _databaseContext.Entries.Remove(entry);
                await _databaseContext.SaveChangesAsync();
            }
            return entry;
        }

        public async Task<Entry?> UpdateEntry(Entry entry)
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
