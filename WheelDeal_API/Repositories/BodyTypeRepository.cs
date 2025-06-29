using Microsoft.EntityFrameworkCore;
using WheelDeal_API.DbContexts;
using WheelDeal_API.Models;
using WheelDeal_API.Repositories.Interface;

namespace WheelDeal_API.Repositories
{
    public class BodyTypeRepository : IBodyType

    {
        private readonly AppDbContext _context;

        public BodyTypeRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BodyType>> GetBodyTypesAsync()

        {
           var bodytype = await _context.BodyType.ToListAsync();
            return bodytype;
        }
    }
}
