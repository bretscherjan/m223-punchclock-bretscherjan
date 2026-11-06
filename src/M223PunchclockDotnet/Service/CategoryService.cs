using M223PunchclockDotnet.Model;
using Microsoft.EntityFrameworkCore;

namespace M223PunchclockDotnet.Service
{
    public class CategoryService
    {
        private readonly DatabaseContext _databaseContext;

        public CategoryService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Task<List<Category>> FindAll()
        {
            return _databaseContext.Categories.ToListAsync();
        }

        public async Task<Category?> FindById(int id)
        {
            return await _databaseContext.Categories.FindAsync(id);
        }

        public async Task<Category> Add(Category category)
        {
            _databaseContext.Categories.Add(category);
            await _databaseContext.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> Update(int id, string title)
        {
            var category = await _databaseContext.Categories.FindAsync(id);
            if (category != null)
            {
                category.Title = title;
                await _databaseContext.SaveChangesAsync();
            }
            return category;
        }

        public async Task<bool> Delete(int id)
        {
            var category = await _databaseContext.Categories.FindAsync(id);
            if (category == null)
            {
                return false;
            }
            _databaseContext.Categories.Remove(category);
            await _databaseContext.SaveChangesAsync();
            return true;
        }
    }
}
