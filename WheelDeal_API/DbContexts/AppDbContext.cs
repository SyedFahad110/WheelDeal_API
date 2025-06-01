using Microsoft.EntityFrameworkCore;
namespace WheelDeal_API.DbContexts
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

    }
}