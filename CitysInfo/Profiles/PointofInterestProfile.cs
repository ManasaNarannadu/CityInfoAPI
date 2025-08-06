using AutoMapper;
using System.Runtime.InteropServices;

namespace CitysInfo.Profiles
{
    public class PointofInterestProfile : Profile
    {

        public PointofInterestProfile()
        {
            CreateMap<Entities.PointsOfInterest, Models.PointofInterestDto>();
        }
    }
}
