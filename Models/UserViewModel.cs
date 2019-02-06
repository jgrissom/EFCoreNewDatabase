using System;
using System.ComponentModel.DataAnnotations;

namespace EFCoreNewDatabase.Models
{
    public class CreateModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [UIHint("email")] // ensures the taghelper renders the appropriate form field
        public string Email { get; set; }

        [Required]
        [UIHint("password")] // ensures the taghelper renders the appropriate form field
        public string Password { get; set; }
    }
}
