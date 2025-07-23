using CitysInfo.Models;
using CitysInfo.services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CitysInfo.Controllers
{
    [Route("api/cities/{cityId}/pointsOfInterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly CityDataStore _CityDataStore;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,IMailService localMailService,CityDataStore cityDataStore)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _mailService = localMailService ?? throw new ArgumentNullException(nameof(localMailService));

            _CityDataStore = cityDataStore ?? throw new ArgumentNullException(nameof(cityDataStore));
        }

        [HttpGet]
        public ActionResult<IEnumerable<PointofInterestDto>> GetPointsofinterest(int cityId)
        {
            try
            {
                var cities = _CityDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

                 if (cities == null)
                 {
                   // _logger.LogInformation("cities data is null");
                    throw new Exception("is not Not Found");
                   // return NotFound();
                 }

                 return Ok(cities.PointOfInterests);


            }
            catch (Exception ex)
            {
                _logger.LogCritical($"ErrorMessage : error occured while getting points of interest {cityId}:"+ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("{PointofInterestId}",Name = "GetPointOfInterest")]

        public ActionResult<PointofInterestDto> GetPointsOfInterestsById(int cityId, int PointofInterestId)
        {
            var cities = _CityDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (cities == null)
            {
                return NotFound();
            }

            var pointsofInterest = cities.PointOfInterests.FirstOrDefault(c => c.Id == PointofInterestId);

            if (pointsofInterest == null)
            {
                return NotFound();
            }

            return Ok(pointsofInterest);

        }

        [HttpPost]
        public ActionResult<PointofInterestDto> CreatePointOfInterest(int cityId, PointOfInterestCreationDto pointOfInterestCreationDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var city = _CityDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if(city == null)
            {
                return NotFound();
            }

            var maxPointOfInterestId = _CityDataStore.Cities.SelectMany(c => c.PointOfInterests).Max(p => p.Id);
            var finalPointOfInterest = new PointofInterestDto()
            {
                Id = ++maxPointOfInterestId,
                Name = pointOfInterestCreationDto.Name,
                Description = pointOfInterestCreationDto.Discription
            };

            city.PointOfInterests.Add(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    CityId = cityId,
                    pointOfInterestId = finalPointOfInterest.Id
                },
                finalPointOfInterest);

        }

        [HttpPut("{pointsOfInterestId}")]
        public ActionResult UpdatePointsOfInterest(int cityId,int pointOfInterestId,PointsOfInterestUpdateDto pointsOfInterestUpdateDto)
        {
            var city = _CityDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if(city == null)
            {
                return NotFound();
            }

            var pointsOfInterestfromStore = city.PointOfInterests.FirstOrDefault(p => p.Id == pointOfInterestId);
            if (pointsOfInterestfromStore == null)
            {
                return NotFound();
            }

            pointsOfInterestfromStore.Name = pointsOfInterestUpdateDto.Name;
            pointsOfInterestfromStore.Description = pointsOfInterestUpdateDto.Discription;

            return NoContent();

        }

        [HttpPatch("{pointofinterestid}")]
        public ActionResult partiallyUpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointsOfInterestUpdateDto> patchDocument)
        {
            var city = _CityDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var pointofIntersetfromStore = city.PointOfInterests.FirstOrDefault(p => p.Id == pointOfInterestId);
            if (pointofIntersetfromStore == null){
                return NotFound();
            }
            var pointOfInterestToPatch = new PointsOfInterestUpdateDto()
            {
                Name = pointofIntersetfromStore.Name,
                Discription = pointofIntersetfromStore.Description
            };

            patchDocument.ApplyTo(pointOfInterestToPatch);
            

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }
            pointofIntersetfromStore.Name = pointOfInterestToPatch.Name;
            pointofIntersetfromStore.Description = pointOfInterestToPatch.Discription;

            return NoContent();
        }

        [HttpDelete("{pointofInterestId}")]

        public ActionResult Delete(int cityId, int pointofInterestId)
        {
            var city = _CityDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var pointofIntersetfromStore = city.PointOfInterests.FirstOrDefault(p => p.Id == pointofInterestId);
            if (pointofIntersetfromStore == null)
            {
                return NotFound();
            }

            city.PointOfInterests.Remove(pointofIntersetfromStore);

            _mailService.Send("point of interest is deleted", $"points of interests {pointofIntersetfromStore.Name} with id {pointofIntersetfromStore.Id} was deleted");
            return NoContent();
        }


    }
}
