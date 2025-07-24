using WheelDeal_API.Models;

namespace WheelDeal_API.Repositories.Interface
{
    public interface ICarColors
    {
        Task<IEnumerable<CarColors>> GetAllCarColorsAsync();
    }
}
