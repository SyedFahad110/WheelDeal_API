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
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // ✅ Correctly hash the raw password (not password hash field)
                signup.PasswordHash = GeneralClass.HashPassword(signup.PasswordHash);

                // ✅ Encrypt Email and encode as Base64
                var encryptedEmail = await GeneralClass.EncryptAsync(signup.Email);
                signup.Email = BitConverter.ToString(encryptedEmail);

                // ✅ Encrypt Phone and encode as Base64
                var encryptedPhone = await GeneralClass.EncryptAsync(signup.Phone);
                signup.Phone = BitConverter.ToString(encryptedPhone);

                _context.SignUp.Add(signup);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                //msla kaha aa rha tha
                //    wait run kar k dhekho shayad thek kar diya tha mainen
                return signup;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Failed to save data. See inner exception for details.", ex);
            }
        }

        public async Task<List<string>> GetAllEncryptedPhoneAsync()
        {
            return await _context.SignUp
                .Where(s => s.IsDeleted == 0 && s.IsActive == 0)
                .Select(s => s.Phone)
                .ToListAsync();
        }

        public async Task<List<string>> GetAllEncryptedEmailAsync()
        {
            return await _context.SignUp
                .Where(s => s.IsDeleted == 0 && s.IsActive == 0)
                .Select(s => s.Email)
                .ToListAsync();
        }

        public async Task<SignUp> GetUserByEmail(string email)
        {
            var user = await _context.SignUp
         .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return null;

            return user;
        }

        public async Task<SignUp> GetUserByIdAsync(int id)
        {
            var user = await _context.SignUp.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return null;
            return user;
        }

        public async Task<bool?> DeleteAsync(int id, string password)
        {
            var user = await _context.SignUp
       .FirstOrDefaultAsync(u => u.Id == id && u.PasswordHash == password && u.IsDeleted == 0);

            if (user == null)
                return null;

            user.IsDeleted = 1;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
//