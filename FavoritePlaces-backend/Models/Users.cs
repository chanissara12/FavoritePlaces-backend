using System.ComponentModel.DataAnnotations;

namespace FavoritePlacesApi.Models
{
    public class Users
    {
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
    }
}
