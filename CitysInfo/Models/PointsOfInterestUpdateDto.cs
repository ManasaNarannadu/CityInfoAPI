using System.ComponentModel.DataAnnotations;

namespace CitysInfo.Models
{
    public class PointsOfInterestUpdateDto
    {
        [Required(ErrorMessage = "You should provide a name value.")]
        [MaxLength(10)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Discription { get; set; }
    }
}
