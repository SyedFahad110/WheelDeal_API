using WheelDeal_API.Models;

namespace WheelDeal_API.Repositories.Interface
{
    public interface IFuelType


    {

       Task<IEnumerable<FuelType>> GetAllFuelType();
    }
}
