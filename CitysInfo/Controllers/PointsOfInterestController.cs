using Asp.Versioning;
using AutoMapper;
using CitysInfo.Entities;
using CitysInfo.Models;
using CitysInfo.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;

namespace CitysInfo.Controllers
{
    [Route("api/v{version:apiVersion}/cities/{cityId}/pointsOfInterest")]

   // [Authorize(Policy = "MustBeFromAntwerp")]
    [ApiController]
    [ApiVersion(2)]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
            IMailService localMailService,
            ICityInfoRepository cityInfoRepository,
            IMapper mapper)
        {
            _logger = logger ?? 
                throw new ArgumentNullException(nameof(logger));

            _mailService = localMailService ?? 
                throw new ArgumentNullException(nameof(localMailService));

            _cityInfoRepository = cityInfoRepository ?? 
                throw new ArgumentNullException(nameof(cityInfoRepository));

            _mapper = mapper ?? 
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointofInterestDto>>> GetPointsofinterest(int cityId)
        {
            var cityName = User.Claims.FirstOrDefault(c => c.Type == "city")?.Value;
            if(!await _cityInfoRepository.CityNameMatchsCityID(cityName, cityId))
            {
                return Forbid();
            }
            try
            {
                var PointsOfInterest = await _cityInfoRepository.GetPointsOfInterestsCityIdAsync(cityId);

                if (PointsOfInterest == null)
                {
                    // _logger.LogInformation("cities data is null");
                    throw new Exception("is not Not Found");
                    // return NotFound();
                }

                return Ok(_mapper.Map<IEnumerable<PointofInterestDto>>(PointsOfInterest));


            }
            catch (Exception ex)
            {
                _logger.LogCritical($"ErrorMessage : error occured while getting points of interest {cityId}:"+ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("{PointofInterestId}",Name = "GetPointOfInterest")]

        public async Task<ActionResult<PointofInterestDto>> GetPointsOfInterestsById(int cityId, int PointofInterestId)
        {

            bool cities = await _cityInfoRepository.CityExistAsync(cityId);
            if (!cities)
            {
                return NotFound();
            }

            var pointsofInterest = await _cityInfoRepository.GetPointsOfInterestsAsync(cityId, PointofInterestId);

            if (pointsofInterest == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PointsOfInterest>(pointsofInterest));

        }

        [HttpPost]
        public async Task<ActionResult<PointofInterestDto>> CreatePointOfInterest(int cityId, PointOfInterestCreationDto pointOfInterestCreationDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if(!await _cityInfoRepository.CityExistAsync(cityId))
            {
                return NotFound();
            } 
            var finalpointsofInterest = _mapper.Map<Entities.PointsOfInterest>(pointOfInterestCreationDto);

            await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, finalpointsofInterest);

            await _cityInfoRepository.SaveChangesAsync();

            var CreatedPointOfInterestToReturn = _mapper.Map<Models.PointofInterestDto>(finalpointsofInterest);

            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    CityId = cityId,
                    pointOfInterestId = CreatedPointOfInterestToReturn.Id
                },
                CreatedPointOfInterestToReturn);

        }

        [HttpPut("{pointsOfInterestId}")]
        public async Task<ActionResult> UpdatePointsOfInterest(int cityId,int pointOfInterestId,PointsOfInterestUpdateDto pointsOfInterestUpdateDto)
        {
            var city = _cityInfoRepository.CityExistAsync(cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointsofInterest = await _cityInfoRepository.GetPointsOfInterestsAsync(cityId,pointOfInterestId);

            _mapper.Map(pointsOfInterestUpdateDto, pointsofInterest);

            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();

            //var pointsOfInterestfromStore = city.PointOfInterests.FirstOrDefault(p => p.Id == pointOfInterestId);
            //if (pointsOfInterestfromStore == null)
            //{
            //    return NotFound();
            //}

            //pointsOfInterestfromStore.Name = pointsOfInterestUpdateDto.Name;
            //pointsOfInterestfromStore.Description = pointsOfInterestUpdateDto.Discription;

        }

        [HttpPatch("{pointofinterestid}")]
        public async Task<ActionResult> partiallyUpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointsOfInterestUpdateDto> patchDocument)
        {
            //var city = _CityDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (!await _cityInfoRepository.CityExistAsync(cityId))
            {
                return NotFound();
            }
            var pointsOfInterest = await _cityInfoRepository.GetPointsOfInterestsAsync(cityId, pointOfInterestId);
            if(pointsOfInterest == null)
            {
                return NotFound();
            }
          
            var pointsOfInterestToPatch = _mapper.Map<PointsOfInterestUpdateDto>(pointsOfInterest);

            // patchDocument.ApplyTo(pointsOfInterestToPatch,ModelState);


            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!TryValidateModel(pointsOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(pointsOfInterestToPatch,pointsOfInterest);
          
            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{pointofInterestId}")]

        public async Task<ActionResult> Delete(int cityId, int pointofInterestId)
        {
            if (!await _cityInfoRepository.CityExistAsync(cityId))
            {
                return NotFound();
            }
            var pointsOfInterest = await _cityInfoRepository.GetPointsOfInterestsAsync(cityId, pointofInterestId);
            if (pointsOfInterest == null)
            {
                return NotFound();
            }

            await _cityInfoRepository.DeletePointOfInterest(cityId, pointsOfInterest);
            await _cityInfoRepository.SaveChangesAsync();

            _mailService.Send(
                "point of interest is deleted", 
                $"points of interests {pointsOfInterest.Name} with id {pointsOfInterest.Id} was deleted");
            return NoContent();
        }


    }
}
