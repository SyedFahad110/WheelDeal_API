using BookPlazaAPI.AppClasses;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WheelDeal_API.DbContexts;
using WheelDeal_API.Models;
using WheelDeal_API.Repositories.Interface;
using WheelDeal_API.Utilitties;

namespace WheelDeal_API.Repositories
{
    public class SignInRepository : ISignIn
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public SignInRepository(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> LoginAsync(SignInModel signInModel)
        {
            // Encrypt the email from the login form to match stored Base64-encrypted value
            var encryptedEmail = await GeneralClass.EncryptAsync(signInModel.Email);
            string base64Email = Convert.ToBase64String(encryptedEmail);


            // Search for a user with the matching encrypted email
            var user = _context.SignUp.FirstOrDefault(u => u.Email == base64Email);
            if (user == null)
                return null;

            // (Optional) Decrypt the user's stored email to display/use elsewhere
            string decryptedEmail = await GeneralClass.DecryptAsync(Convert.FromBase64String(user.Email));

            // Verify password
            bool isValid = PasswordHasher.VerifyPassword(signInModel.Password, user.PasswordHash); // Correct order
            if (!isValid)
                return null;
            // Generate JWT token
            var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, decryptedEmail), // use decrypted email in token
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
//
