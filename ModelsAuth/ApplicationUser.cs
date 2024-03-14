using Microsoft.AspNetCore.Identity;
using PortalVioo.ModelsApp;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalVioo.Models
{
    [Table("AspNetUsers")]
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<MembreProjet> MembreProjet { get; set; }
        public virtual ICollection<Tache> Tache { get; set; }

    }
}
