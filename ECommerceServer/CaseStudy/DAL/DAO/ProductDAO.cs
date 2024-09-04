using CaseStudy.DAL.DomainClasses;
using Microsoft.EntityFrameworkCore;

namespace CaseStudy.DAL.DAO
{
    public class ProductDAO
    {
        private readonly AppDbContext _db;
        public ProductDAO(AppDbContext btx)
        {
            _db = btx;
        }
        public async Task<List<Product>> GetAllByBrand(int id)
        {
            return await _db.Products!.Where(item => item.BrandId! == id).ToListAsync();
        }

    }
}
