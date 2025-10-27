using Asp.Versioning;
using AutoMapper;
using CitysInfo.Entities;
using CitysInfo.Models;
using CitysInfo.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CitysInfo.Controllers
{
    [ApiController]
   // [Authorize]
    [Route("api/v{version:apiVersion}/cities")]
    [ApiVersion(1)]
    [ApiVersion(2)]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;

        public CitiesController(ICityInfoRepository cityInfoRepository,IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(Name = "GetCities")]
        public async Task<ActionResult<CitywithoutPointsOfInterestDTO>> GetCities(string? name,string? searchQuery,int pageNumber = 1,int pageSize = 10)
        {

            if(pageSize > maxCitiesPageSize)
            {
                pageSize = maxCitiesPageSize;
            }
            var (cityEntities, paginationMetadata) = await _cityInfoRepository
                .GetCitiesAsync(name, searchQuery,pageNumber,pageSize); 

            Response.Headers.Add("X-Pagination",JsonSerializer.Serialize(paginationMetadata));

            return Ok(_mapper.Map<IEnumerable<CitywithoutPointsOfInterestDTO>>(cityEntities));
           // return Ok(_CityDataStore.Cities);
        }
        /// <summary>
        /// Get a City by Id
        /// </summary>
        /// <param name="id">The id of the city to get</param>
        /// <param name="includePointsOfInterest">Whether or not to include points of interest</param>
        /// <returns>A city with or without points of interest</returns>
        /// <response code="200">Returns the requested city</response>

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddCities(int id, bool includePointsOfInterest = false)
        {
            var city = await _cityInfoRepository.GetCityByIdAsync(id, includePointsOfInterest);
            //var CitiesToReturn = _CityDataStore.Cities.FirstOrDefault(c => c.Id == id);

            if (city == null)
            {
                return NotFound();
            }
                
            if (includePointsOfInterest)
            {
                return Ok(_mapper.Map<CityDto>(city));
            }
            return Ok(_mapper.Map<CitywithoutPointsOfInterestDTO>(city));
        }

        [HttpPost]
        public async Task<IActionResult> AddCities(CityCreationDTO cityData)
        {
            var newCity = _mapper.Map<City>(cityData);
            await _cityInfoRepository.AddCity(newCity);
            await _cityInfoRepository.SaveChangesAsync();
            _mapper.Map<CityDto>(newCity);
            return CreatedAtRoute("GetCities",new { Id = newCity.Id },newCity);
        }
    }
}
