using AutoMapper;
using CitysInfo.Entities;
using CitysInfo.Models;
using CitysInfo.services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CitysInfo.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public CitiesController(ICityInfoRepository cityInfoRepository,IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet()]
        public async Task<ActionResult<City>> GetCities()
        {
            var cityEntities = await _cityInfoRepository.GetCitiesAsync();
            
            return Ok(_mapper.Map<IEnumerable<City>>(cityEntities));
           // return Ok(_CityDataStore.Cities);
        }

        [HttpGet("{id}")]
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
    }
}
