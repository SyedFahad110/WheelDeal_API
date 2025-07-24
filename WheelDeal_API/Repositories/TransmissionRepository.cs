using WheelDeal_API.DbContexts;
using WheelDeal_API.Models;
using WheelDeal_API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace WheelDeal_API.Repositories
{
    public class TransmissionRepository: ITransmission
    {
        private readonly AppDbContext _context;

        public TransmissionRepository(AppDbContext context) { 
         
            _context = context;
        
        }

        public async Task<IEnumerable<Transmission>> GetTransmissionsAsync()
        {
            var transmissions = await _context.Transmission.ToListAsync();

            return transmissions;
        }
    }
}
