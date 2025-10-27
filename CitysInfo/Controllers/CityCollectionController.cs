using AutoMapper;
using CitysInfo.Models;
using CitysInfo.services;
using Microsoft.AspNetCore.Mvc;

namespace CitysInfo.Controllers
{

    [ApiController]
    // [Authorize]
    [Route("api/v{version:apiVersion}/citycollection")]
    public class CityCollectionController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;

        public CityCollectionController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<CityCreationDTO>>> CreateCityCollection(IEnumerable<CityCreationDTO> citycollection)
        {
            var cityEntities = _mapper.Map<IEnumerable<Entities.City>>(citycollection);
            foreach (var entity in cityEntities)
            {
                _cityInfoRepository.AddCity(entity);
            }
            await _cityInfoRepository.SaveChangesAsync();
            return Ok();

        }

    }
}
