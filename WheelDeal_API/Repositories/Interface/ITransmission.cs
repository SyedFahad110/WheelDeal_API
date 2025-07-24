using WheelDeal_API.Models;

namespace WheelDeal_API.Repositories.Interface
{
    public interface ITransmission
    {
       Task <IEnumerable<Transmission>> GetTransmissionsAsync ();
    }
}
