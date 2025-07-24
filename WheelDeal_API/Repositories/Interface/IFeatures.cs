using WheelDeal_API.Models;

namespace WheelDeal_API.Repositories.Interface
{
    public interface IFeatures
    {
        Task<IEnumerable<Features>> GetAllFeatures();
    }
}
