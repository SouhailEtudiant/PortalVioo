using PortalVioo.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PortalVioo.ModelsApp
{
    public class Imputation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateOnly date {  get; set; }

        public decimal chargeEnHeure { get; set;  }

        public bool IsActive { get; set; }

        public string IdUtilisateur { get; set; }

        [ForeignKey("IdUtilisateur")]
        public virtual ApplicationUser? ApplicationUser { get; set; }

        [Required]
        public int IdTache { get; set; }

        [ForeignKey("IdTache")]
        public virtual Tache? Tache { get; set; }
    }
}
