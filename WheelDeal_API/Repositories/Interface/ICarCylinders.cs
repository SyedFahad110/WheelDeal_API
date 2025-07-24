using WheelDeal_API.Models;
namespace WheelDeal_API.Repositories.Interface
{
    public interface ICarCylinders
    {
        Task<IEnumerable<CarCylinders>> GetAllCarCylinders();
    }
}
