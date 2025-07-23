using CitysInfo.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitysInfo.Entities
{
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public City(string name)
        {
            Name = name;
        }

        [MaxLength(100)]
        public string Description { get; set; }

        public ICollection<PointofInterestDto> PointOfInterests { get; set; } = new List<PointofInterestDto>();


    }
}
