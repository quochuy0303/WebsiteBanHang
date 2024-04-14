using Microsoft.EntityFrameworkCore;
using WebsiteBanHang.Interface;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Repository
{
    public class CatologyRepository : ICategoryRepository
    {
        private readonly WebsiteBanHangContext _context;
        public CatologyRepository(WebsiteBanHangContext context)
        {
            _context = context;
        }
        // Tương tự như EFProductRepository, nhưng cho Category
        public async Task<IEnumerable<Catology>> GetAllAsync()
        {
            return await _context.Catologies.ToListAsync();
        }
        public async Task<Catology> GetByIdAsync(int id)
        {
            return await _context.Catologies.FindAsync(id);
        }
        public async Task AddAsync(Catology category)
        {
            _context.Catologies.Add(category);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Catology category)
        {
            _context.Catologies.Update(category);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var category = await _context.Catologies.FindAsync(id);
            _context.Catologies.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
