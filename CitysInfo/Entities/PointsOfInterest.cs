using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CitysInfo.Entities
{
    public class PointsOfInterest
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("cityId")]
        public City? city { get; set; }

        public int cityId { get; set; }

        public PointsOfInterest(string name)
        {
            Name = name;
        }
    }
}
