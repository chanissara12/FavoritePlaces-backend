using System.ComponentModel.DataAnnotations;

namespace FavoritePlacesApi.Models
{
    public class PlacesComment
    {
        [Key]
        public required int user_id { get; set; }
        public required int place_id { get; set; }
        public required float rating { get; set; }
        public string? comment { get; set; }
    }
}
