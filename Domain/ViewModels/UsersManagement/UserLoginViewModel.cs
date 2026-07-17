using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace Domain.ViewModels.UsersManagement
{
    public class UserLoginViewModel
    {
        [Required]
        //[Required(ErrorMessage = "Username is required")]
        [JsonPropertyName("userName")]
        public string userName { get; set; } = string.Empty;

        [Required]
        //[Required(ErrorMessage = "Password is required")]
        [JsonPropertyName("password")]
        public string password { get; set; } = string.Empty;
    }
}
