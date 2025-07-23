using CitysInfo.Entities;
using Microsoft.EntityFrameworkCore;

namespace CitysInfo.DbContexts
{
    public class CityInfoContext : DbContext
    {

        public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options)
        {
        }
        public DbSet<City> Cities {  get; set; }

        public DbSet<PointsOfInterest> PointsOfInterest { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("connectionstring");
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
