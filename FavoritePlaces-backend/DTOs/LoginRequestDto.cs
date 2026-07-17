using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FavoritePlacesApi.DTOs
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Username is required")]
        [JsonPropertyName("user_name")]
        public string user_name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [JsonPropertyName("password")]
        public string password { get; set; } = string.Empty;
    }
}
