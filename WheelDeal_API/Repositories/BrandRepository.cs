using Microsoft.EntityFrameworkCore;
using WheelDeal_API.DbContexts;
using WheelDeal_API.Models;
using WheelDeal_API.Repositories.Interface;

namespace WheelDeal_API.Repositories
{
    public class BrandRepository : IBrand
    {
        private readonly AppDbContext _context;

        public BrandRepository(AppDbContext context)
        {

            _context = context;
        }
        
        public async Task<IEnumerable<Brand>> GetAllBrandAsync()
        {
            var brand = await _context.Brand.ToListAsync();
            return brand;
        }
    }

}
