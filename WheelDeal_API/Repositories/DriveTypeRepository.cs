using WheelDeal_API.Repositories.Interface;
using WheelDeal_API.DbContexts;
using Microsoft.EntityFrameworkCore;
using WheelDeal_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace WheelDeal_API.Repositories
{
    public class DriveTypeRepository :IDriveType
    {
        private readonly AppDbContext _context;

        public DriveTypeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Models.DriveType>> GetDriveTypes()
        {
           var drivetype = await _context.DriveType.ToListAsync();

            return drivetype;
        }

      
    }
}
