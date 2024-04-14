using WebsiteBanHang.Models;

namespace WebsiteBanHang.Interface
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Catology>> GetAllAsync();
        Task<Catology> GetByIdAsync(int id);
        Task AddAsync(Catology category);
        Task UpdateAsync(Catology category);
        Task DeleteAsync(int id);
    }
}
