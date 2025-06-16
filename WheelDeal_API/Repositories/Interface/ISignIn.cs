using WheelDeal_API.Models;

namespace WheelDeal_API.Repositories.Interface
{
    public interface ISignIn
    {
        Task<string> LoginAsync(SignInModel signInModel);
    }
}
