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

        public async Task<Category> Add(Category category)
        {
            _databaseContext.Categories.Add(category);
            await _databaseContext.SaveChangesAsync();
            return category;
        }
    }
}
