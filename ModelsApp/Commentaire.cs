using PortalVioo.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalVioo.ModelsApp
{
    public class Commentaire
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Description { get; set; }
        public DateOnly Date { get; set; }
        public bool IsActive { get; set; }
        public string CreePar { get; set; }

        [ForeignKey("CreePar")]
        public virtual ApplicationUser? ApplicationUser { get; set; }

        public int IdTache { get; set; }

        [ForeignKey("IdTache")]
        public virtual Tache? Tache { get; set; }

    }
}
