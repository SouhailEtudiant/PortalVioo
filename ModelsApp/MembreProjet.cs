using PortalVioo.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PortalVioo.ModelsApp
{
    public class MembreProjet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMembrePorjet { get; set; }
        public string IdUtilisateur { get; set; }

        [ForeignKey("IdUtilisateur")]
        public virtual ApplicationUser? ApplicationUser { get; set; }

        public int IdProjet { get; set; }

        [ForeignKey("IdProjet")]
        public virtual Projet?  Projet { get; set; }

    }
}
