using AutoMapper;
using System.Runtime.InteropServices;

namespace CitysInfo.Profiles
{
    public class PointofInterestProfile : Profile
    {

        public PointofInterestProfile()
        {
            CreateMap<Entities.PointsOfInterest, Models.PointofInterestDto>();

            CreateMap<Models.PointOfInterestCreationDto, Entities.PointsOfInterest>();

            CreateMap<Models.PointsOfInterestUpdateDto, Entities.PointsOfInterest>();

            CreateMap<Entities.PointsOfInterest,Models.PointsOfInterestUpdateDto>();
        }
    }
}
