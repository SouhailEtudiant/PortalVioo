using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PortalVioo.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }


    }
}
