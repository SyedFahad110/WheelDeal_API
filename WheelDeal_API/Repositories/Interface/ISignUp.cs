using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WheelDeal_API.Models;

namespace WheelDeal_API.Repositories.Interface

{
    public interface ISignUp
    {
        Task<SignUp> AddAsync(SignUp user);
        Task<List<string>> GetAllEncryptedPhoneAsync();

        //Task<SignUp> GetByIdAsync(int id);
        //Task<SignUp> GetByEmailAsync(string email);
        //Task<bool> EmailExistsAsync(string email);
        //Task<IEnumerable<SignUp>> GetAllAsync();
        //Task UpdateAsync(SignUp user);
        //Task DeleteAsync(int id) << ye bht agy ka kam hy abi
    }
}


//// ye Inerface ye puchna hai method kya kya honge
//Method mai ne chat gpt se liye hai
//    //abi sy ye sary methods nhi bnaow jin methods ki zrurat ho wo bnaow confuse hojaegi 
//    //tention hojaegi apko k yar ye sary methods ka kaam bi krna hy mjhy
//    //ekk ek method lekr chlo
//    //Add async k andr existing ka function chla dena
