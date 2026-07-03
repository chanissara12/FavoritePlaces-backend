using FavoritePlacesApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FavoritePlacesApi.Data
{
    public class ApplicationDBContext:DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        public DbSet<Users> Users { get; set; }
        public DbSet<Places> Places { get; set; }
        public DbSet<UserFavoritePlaces> UserFavoritePlaces { get; set; }
        public DbSet<PlacesComment> PlacesComment { get; set; }
        public DbSet<AppRole> AppRole { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
    }
}
