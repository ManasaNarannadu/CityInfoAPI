using CitysInfo.Models;

namespace CitysInfo
{
    public class CityDataStore
    {
        public List<CityDto> Cities { get; set; }
       // public static CityDataStore current { get; } = new CityDataStore();
        public CityDataStore() 
        {
            Cities = new List<CityDto>
                       {
    new CityDto()
    {
        Id = 1,
        Name = "New York City",
        Description = "the one with that big park.",
        PointOfInterests = new List<PointofInterestDto>
        {
            new PointofInterestDto()
            {
                Id = 1,
                Name = "centralPark",
                Description = "this park is very peaceful to walk in"
            },
            new PointofInterestDto()
            {
                Id= 2,
                Name = "empire state building",
                Description = "yah its a very good building"
            }
        }
    },
    new CityDto()
    {
        Id = 2,
        Name = "Antwerp",
        Description = "the one with the cathedral that was never really finished.",
        PointOfInterests = new List<PointofInterestDto>
        {
            new PointofInterestDto()
            {
                Id = 1,
                Name = "centralPark",
                Description = "this park is very peaceful to walk in"
            },
            new PointofInterestDto()
            {
                Id= 2,
                Name = "empire state building",
                Description = "yah its a very good building"
            }
        }
    },
    new CityDto()
    {
        Id = 3,
        Name = "Paris",
        Description = "the one with that big tower.",
        PointOfInterests = new List<PointofInterestDto>()
        {
             new PointofInterestDto()
            {
                Id = 1,
                Name = "centralPark",
                Description = "this park is very peaceful to walk in"
            },
            new PointofInterestDto()
            {
                Id= 2,
                Name = "empire state building",
                Description = "yah its a very good building"
            }
        }
    }
};
        }
    }
}
