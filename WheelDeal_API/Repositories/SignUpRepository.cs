using BookPlazaAPI.AppClasses;
using Microsoft.EntityFrameworkCore;
using WheelDeal_API.DbContexts;
using WheelDeal_API.Models;
using WheelDeal_API.Repositories.Interface;
using WheelDeal_API.Utilitties;

namespace WheelDeal_API.Repositories
{
    public class SignUpRepository : ISignUp
    {
        private readonly AppDbContext _context;
        public SignUpRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<SignUp> AddAsync(SignUp signup)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    PasswordHasher Password = new PasswordHasher();

                    //signup.PasswordHash = PasswordHasher.HashPassword(signup.Password);
                    signup.PasswordHash = PasswordHasher.HashPassword(signup.PasswordHash);

                    var EncryptEmail = await GeneralClass.EncryptAsync(signup.Email);
                    signup.Email = BitConverter.ToString(EncryptEmail);

                    var EncryptContactNo = await GeneralClass.EncryptAsync(signup.Phone);
                    signup.Phone = BitConverter.ToString(EncryptContactNo);

                    
                    _context.SignUp?.Add(signup);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return signup;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Failed to save data. See inner exception for details.", ex);
                }
            }
        }

        public async Task<List<string>> GetAllEncryptedPhoneAsync()
        {
            return await _context.SignUp.Where(s => s.IsDeleted == 0 && s.IsActive == 0)
                .Select(s => s.Phone)
                .ToListAsync();
        }
    }
}
