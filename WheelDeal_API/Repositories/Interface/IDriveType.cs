using WheelDeal_API.Models;

namespace WheelDeal_API.Repositories.Interface
{
    public interface IDriveType
    {

        Task<IEnumerable<Models.DriveType>> GetDriveTypes();
        //Task<IEnumerable<Brand>> GetAllBrandAsync();
    }
}
