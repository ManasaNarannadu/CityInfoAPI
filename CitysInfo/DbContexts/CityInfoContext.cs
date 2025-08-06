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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
                new City("New York City")
                {
                    Id = 1,
                    Description = "The one with that big park. "
                },
                new City("Antwerp")
                {
                    Id = 2,
                    Description = "The one with the cathedral that was never really finished. "
                },
                new City("Paris")
                {
                    Id = 3,
                    Description = "the onw with that big tower. "
                });
                
            modelBuilder.Entity<PointsOfInterest>().HasData(
                new PointsOfInterest("Central Park")
                {
                    Id = 1,
                    cityId = 1,
                    Discription = "the most visited unbrun park in the united states."
                },
                new PointsOfInterest("Empire State building")
                { 
                    Id = 2,
                    cityId=1,
                    Discription = "A 102-story skycraper located in midtown manhattan."
                },
                new PointsOfInterest("cathdral")
                { 
                    Id = 3,
                    cityId=2,
                    Discription = "A gothic style cathdral, concevied by architects."
                },
                new PointsOfInterest("efil tower")
                {
                    Id = 4,
                    cityId = 3,
                    Discription = "most beautiful and highest tower in world."
                });
                
            base.OnModelCreating(modelBuilder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("connectionstring");
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
