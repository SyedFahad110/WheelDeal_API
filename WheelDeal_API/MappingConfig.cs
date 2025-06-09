using WheelDeal_API.Models;
using WheelDeal_API.Repositories.DTO;
using AutoMapper;

namespace WheelDeal_API
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<SignUp, DTOSignUp>().ReverseMap();

        }
    }
}
