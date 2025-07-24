using Microsoft.EntityFrameworkCore;
using WheelDeal_API.DbContexts;
using WheelDeal_API.Models;
using WheelDeal_API.Repositories.Interface;


namespace WheelDeal_API.Repositories
{
    public class CarCylindersRepository: ICarCylinders
    {
        private readonly AppDbContext _context;

        public CarCylindersRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CarCylinders>> GetAllCarCylinders()
        {
            var cylinder = await _context.CarCylinders.ToListAsync();

            return cylinder;
        }
    }
}
