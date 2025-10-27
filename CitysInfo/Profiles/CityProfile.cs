using AutoMapper;

namespace CitysInfo.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile() 
        {
           CreateMap<Entities.City,Models.CitywithoutPointsOfInterestDTO>();
            CreateMap<Entities.City, Models.CityDto>();
            CreateMap<Entities.City,Models.CityCreationDTO>();
            CreateMap<Models.CityCreationDTO,Entities.City>();
        }
    }
}
