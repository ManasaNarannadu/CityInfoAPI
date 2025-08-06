using CitysInfo.Entities;

namespace CitysInfo.services
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();

        Task<City> GetCityByIdAsync(int cityid,bool includePointsOfInterest);

        Task<IEnumerable<PointsOfInterest>> GetPointsOfInterestsCityIdAsync(int cityid);
        
        Task<PointsOfInterest> GetPointsOfInterestsAsync(int cityid, int pointsOfInterestsId);
        Task<bool> CityExistAsync(int cityId);
    }
}
