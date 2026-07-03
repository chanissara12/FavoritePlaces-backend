using System.ComponentModel.DataAnnotations;

namespace FavoritePlacesApi.Models
{
    public class UserFavoritePlaces
    {
        [Key]
        public required int user_id { get; set; }
        public required int place_id { get; set; }
    }
}
