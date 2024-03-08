using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalVioo.Models
{
    [Table("AspNetUsers")]
    public class ApplicationUser : IdentityUser
    {
    }
}
