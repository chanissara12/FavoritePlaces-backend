using System.ComponentModel.DataAnnotations;

namespace FavoritePlacesApi.Models
{
    public class UserRole
    {
        [Key]
        public required int user_id { get; set; }
        public required string role_id { get; set; }
    }
}
