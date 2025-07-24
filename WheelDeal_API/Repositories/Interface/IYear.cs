using Org.BouncyCastle.Bcpg.OpenPgp;
using WheelDeal_API.Models;

namespace WheelDeal_API.Repositories.Interface
{
    public interface IYear
    {
        Task<IEnumerable<Year>> GetAllYearAsync();
    }
}
