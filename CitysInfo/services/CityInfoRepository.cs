using CitysInfo.DbContexts;
using CitysInfo.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
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

        public async Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(string? name,string? searchQuery,int pageNumber, int pageSize)
        {
            //collection to start from
            var collection = _context.Cities as IQueryable<City>;

            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                collection = collection.Where(c => c.Name == name);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(a => a.Name.Contains(searchQuery) 
                || (a.Description != null && a.Description.Contains(searchQuery)));
            }

            var totalItemCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await collection.OrderBy(c => c.Name)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (collectionToReturn, paginationMetadata);
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

        public async Task<bool> CityNameMatchsCityID(string? cityName,int cityId)
        {
            return await _context.Cities.AnyAsync(c => c.Id == cityId && c.Name == cityName);
        } 

        public async Task<PointsOfInterest> GetPointsOfInterestsAsync(int cityid, int pointsOfInterestsId)
        {
            return await _context.PointsOfInterest.Where(p => p.cityId == cityid && p.Id == pointsOfInterestsId).FirstOrDefaultAsync();
        }

        public async Task<bool> CityExistAsync(int cityId)
        {
            return await _context.Cities.AnyAsync(c => c.Id == cityId);
        }

        public async Task AddPointOfInterestForCityAsync(int cityid, PointsOfInterest pointOfinterest)
        {
            var city = await GetCityByIdAsync(cityid, false);
            if (city != null)
            {
                city.PointOfInterests.Add(pointOfinterest);
            }

        }

        public async Task DeletePointOfInterest(int cityId,PointsOfInterest PointofInterest)
        {
            var city = await GetCityByIdAsync(cityId, false);
            if (city != null)
            {
                city.PointOfInterests.Remove(PointofInterest);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()>=0);
        }
    }
}
