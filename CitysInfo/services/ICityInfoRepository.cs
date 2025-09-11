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

        Task AddPointOfInterestForCityAsync(int cityid,PointsOfInterest point);

        Task<bool> SaveChangesAsync();
        Task DeletePointOfInterest(int cityId, PointsOfInterest PointofInterest);
        Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync
            (string? name,string? searchQuery,int pageNumber,int pageSize);
        Task<bool> CityNameMatchsCityID(string? cityName, int ctyId); 
    }
}
