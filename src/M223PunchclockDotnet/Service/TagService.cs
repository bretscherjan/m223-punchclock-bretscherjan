using M223PunchclockDotnet.Model;
using Microsoft.EntityFrameworkCore;

namespace M223PunchclockDotnet.Service
{
    public class TagService
    {
        private readonly DatabaseContext _databaseContext;

        public TagService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Task<List<Tag>> FindAll()
        {
            return _databaseContext.Tags.ToListAsync();
        }

        public async Task<Tag?> FindById(int id)
        {
            return await _databaseContext.Tags.FindAsync(id);
        }

        public async Task<Tag> Add(Tag tag)
        {
            _databaseContext.Tags.Add(tag);
            await _databaseContext.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> Update(int id, string title)
        {
            var tag = await _databaseContext.Tags.FindAsync(id);
            if (tag != null)
            {
                tag.Title = title;
                await _databaseContext.SaveChangesAsync();
            }
            return tag;
        }

        public async Task<bool> Delete(int id)
        {
            var tag = await _databaseContext.Tags.FindAsync(id);
            if (tag == null)
            {
                return false;
            }
            _databaseContext.Tags.Remove(tag);
            await _databaseContext.SaveChangesAsync();
            return true;
        }
    }
}
