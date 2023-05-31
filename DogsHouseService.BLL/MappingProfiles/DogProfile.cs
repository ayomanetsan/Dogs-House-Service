using AutoMapper;
using DogsHouseService.Common.DTO.Dog;
using DogsHouseService.DAL.Entities;

namespace DogsHouseService.BLL.MappingProfiles
{
    internal class DogProfile : Profile
    {
        public DogProfile()
        {
            CreateMap<Dog, DogDto>();
        }
    }
}
