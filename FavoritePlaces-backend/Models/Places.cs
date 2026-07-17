using System.ComponentModel.DataAnnotations;

namespace FavoritePlacesApi.Models
{
    public class Places
    {
        public int place_id { get; set; }
        public required string title { get; set; }
        public required string img_src { get; set; }
        public required string img_alt { get; set; }
        public required string add_by { get; set; }
        public required char isApproved { get; set; }
    }
}
