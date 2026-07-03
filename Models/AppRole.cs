using System.ComponentModel.DataAnnotations;

namespace FavoritePlacesApi.Models
{
    public class AppRole
    {
        [Key]
        public string role_id { get; set; }
        public string role_name { get; set; }
    }
}
