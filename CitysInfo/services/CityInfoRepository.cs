using CitysInfo.DbContexts;
using CitysInfo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SQLitePCL;

namespace CitysInfo.services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _context.Cities.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<City> GetCityByIdAsync(int cityid, bool includePointsOfInterest)
        {
            if(includePointsOfInterest)
            {
                return await _context.Cities.Include(c => c.PointOfInterests).Where(c => c.Id == cityid).FirstOrDefaultAsync();
            }
            return await _context.Cities.Where(c => c.Id == cityid).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointsOfInterest>> GetPointsOfInterestsCityIdAsync(int cityid)
        {
            return await _context.PointsOfInterest.Where(p => p.cityId == cityid).ToListAsync();
        }

        public async Task<PointsOfInterest> GetPointsOfInterestsAsync(int cityid, int pointsOfInterestsId)
        {
            return await _context.PointsOfInterest.Where(p => p.cityId == cityid && p.Id == pointsOfInterestsId).FirstOrDefaultAsync();
        }

        public async Task<bool> CityExistAsync(int cityId)
        {
            return await _context.Cities.AnyAsync(c => c.Id == cityId);
        }
    }
}
