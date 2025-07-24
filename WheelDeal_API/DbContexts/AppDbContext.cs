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

        public DbSet<BodyType> BodyType { get; set; }

        public DbSet<Brand> Brand { get; set; }
       
        public DbSet<Models.DriveType> DriveType { get; set; }

        public DbSet<FuelType> FuelType { get; set; }
        public DbSet<Model> Model { get; set; }
        public DbSet<Year> Year { get; set; }
        public DbSet<Condition> Condition { get; set; }
        public DbSet<CarCylinders> CarCylinders { get; set; }
        public DbSet<CarColors> CarColors { get; set; }
        public DbSet<Transmission> Transmission { get; set; }
        public DbSet<Features> Features { get; set; }


    }
}