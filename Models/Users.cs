using System.ComponentModel.DataAnnotations;

namespace FavoritePlacesApi.Models
{
    public class Users
    {
        [Key]
        public int user_id { get; set; }
        [Required]
        public string user_name { get; set; }
        public string password { get; set; }
    }
}
