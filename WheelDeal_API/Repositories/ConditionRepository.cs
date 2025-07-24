using MimeKit.Cryptography;
using WheelDeal_API.DbContexts;
using WheelDeal_API.Models;
using WheelDeal_API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace WheelDeal_API.Repositories
{
    public class ConditionRepository : ICondition
    {
        private readonly AppDbContext _context;

        public ConditionRepository(AppDbContext context) 
        {
           _context = context;
        }

        public async Task<IEnumerable<Condition>> GetConditionsAsync()
        {
            var condition = await _context.Condition.ToListAsync();
            
            return condition;
        }
    }
}
