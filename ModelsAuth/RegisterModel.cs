using System.ComponentModel.DataAnnotations;

namespace PortalVioo.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public required string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Nom is required")]
        public string NomUser { get; set; }

        [Required(ErrorMessage = "Prenom is required")]
        public string PrenomUser { get; set; }

        public string phone { get; set; }

        public string? ImagePath { get; set; }
    }
}
