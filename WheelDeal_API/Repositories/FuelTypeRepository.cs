using Microsoft.EntityFrameworkCore;
using WheelDeal_API.DbContexts;
using WheelDeal_API.Repositories.Interface;
using WheelDeal_API.Models;

namespace WheelDeal_API.Repositories
{
    public class FuelTypeRepository: IFuelType
    {
        private readonly AppDbContext _context;

        public FuelTypeRepository(AppDbContext context)
        {

            _context = context;
        }

        public async Task <IEnumerable<FuelType>> GetAllFuelType()
        {
            var fueltype = await _context.FuelType.ToListAsync();
            return fueltype;
        }
    }
}
