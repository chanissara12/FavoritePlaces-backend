using System.ComponentModel.DataAnnotations;

namespace FavoritePlacesApi.Models
{
    public class UserRole
    {
        public required int user_id { get; set; }
        public required string role_id { get; set; }
    }
}
