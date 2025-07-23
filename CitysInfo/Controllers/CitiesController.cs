using CitysInfo.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CitysInfo.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly CityDataStore _CityDataStore;

        public CitiesController(CityDataStore cityDataStore)
        {
            _CityDataStore = cityDataStore ?? throw new ArgumentNullException(nameof(CityDataStore));
        }

        [HttpGet()]
        public ActionResult<CityDataStore> GetCities()
        {
            return Ok(_CityDataStore.Cities);
        }

        [HttpGet("{id}")]
        public ActionResult<CityDto> AddCities(int id)
        {
            var CitiesToReturn = _CityDataStore.Cities.FirstOrDefault(c => c.Id == id);

            if (CitiesToReturn == null)
            {
                return NotFound();
            }

            return Ok( CitiesToReturn );
        }
    }
}
