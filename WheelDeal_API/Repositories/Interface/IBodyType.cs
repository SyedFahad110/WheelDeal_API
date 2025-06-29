using WheelDeal_API.Models;

namespace WheelDeal_API.Repositories.Interface
{
    public interface IBodyType

    {
        Task<IEnumerable<BodyType>> GetBodyTypesAsync();
    }
}
