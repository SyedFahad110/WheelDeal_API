using Microsoft.EntityFrameworkCore;
using WheelDeal_API.DbContexts;
using WheelDeal_API.Models;
using WheelDeal_API.Repositories.Interface;

namespace WheelDeal_API.Repositories
{

    public class YearRepository : IYear
    {

        private readonly AppDbContext _context;
        public YearRepository(AppDbContext context) {
            _context = context; 
        }
        public async Task<IEnumerable<Year>> GetAllYearAsync()
        {
            var year = await _context.Year.ToListAsync();
            return year;
        }
    }
}
