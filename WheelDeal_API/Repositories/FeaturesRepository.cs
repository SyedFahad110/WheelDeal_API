using WheelDeal_API.DbContexts;
using WheelDeal_API.Models;
using WheelDeal_API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace WheelDeal_API.Repositories
{
    public class FeaturesRepository: IFeatures

    {
        private readonly AppDbContext _context;

        public FeaturesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Features>> GetAllFeatures()
        {
            var features = await _context.Features.ToListAsync();

            return features;
        }
    }
}
