using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MimeKit.Cryptography;
using WheelDeal_API.DbContexts;
using WheelDeal_API.Models;
using WheelDeal_API.Repositories.Interface;

namespace WheelDeal_API.Repositories
{
    public class ModelRepository: IModels

    {
        private readonly AppDbContext _context;

        public ModelRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Model>> GetAllModel()
        {
            var model = await _context.Model.ToListAsync();

            return model;
        
        }
    }
}
