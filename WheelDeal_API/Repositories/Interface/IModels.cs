using WheelDeal_API.Models;

namespace WheelDeal_API.Repositories.Interface
{
    public interface IModels
    {
        Task<IEnumerable<Model>> GetAllModel();
    }
}
