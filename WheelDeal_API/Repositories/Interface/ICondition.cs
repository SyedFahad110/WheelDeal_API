
using WheelDeal_API.Models;

namespace WheelDeal_API.Repositories.Interface
{
    public interface ICondition
    {
        Task <IEnumerable<Condition>> GetConditionsAsync ();   
    }
}
