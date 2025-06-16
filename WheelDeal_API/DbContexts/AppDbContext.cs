using Microsoft.EntityFrameworkCore;
using WheelDeal_API.Models;

namespace WheelDeal_API.DbContexts
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<SignUp> SignUp { get; set; }
        public DbSet<SignInModel> SignInModel { get; set; }

    }
}
