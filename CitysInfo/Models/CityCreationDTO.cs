using CitysInfo.Entities;
using System.ComponentModel.DataAnnotations;

namespace CitysInfo.Models
{
    public class CityCreationDTO
    {
        [Required(ErrorMessage ="you must provide a Name to create a city")]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }

        public ICollection<PointOfInterestCreationDto> PointOfInterests { get; set; } = new List<PointOfInterestCreationDto>();
    }
}
