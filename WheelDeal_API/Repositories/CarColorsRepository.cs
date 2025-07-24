using Microsoft.EntityFrameworkCore;
using WheelDeal_API.DbContexts;
using WheelDeal_API.Models;
using WheelDeal_API.Repositories.Interface;


namespace WheelDeal_API.Repositories
{
    public class CarColorsRepository : ICarColors
    {

        private readonly AppDbContext _context;

        public CarColorsRepository(AppDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<CarColors>> GetAllCarColorsAsync()
        {
            var colors = await _context.CarColors.ToListAsync();

            return colors;
        }
    }
}
