using Microsoft.EntityFrameworkCore;
using Travel_Planner.Models;

namespace Travel_Planner.Data
{

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Itinerary> Itineraries { get; set; }
    }

}