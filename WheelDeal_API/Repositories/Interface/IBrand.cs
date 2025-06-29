using WheelDeal_API.Models;

namespace WheelDeal_API.Repositories.Interface
{
    public interface IBrand
    {
        Task<IEnumerable<Brand>> GetAllBrandAsync();
        

        
    }
}
