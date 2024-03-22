using PortalVioo.Controllers;
using PortalVioo.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalVioo.ModelsApp
{
    public class Tache
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string TacheTitre { get; set; }
        public string TacheDescription { get; set;}

        public decimal ChargeEstime { get; set;}
        public decimal ChargeReele {  get; set;}
        public DateOnly DateDebut { get; set;}
        public DateOnly DateFin { get; set;}
        public bool IsActive { get; set; }
        public virtual ICollection<Imputation>? Imputation { get; set; }
        public virtual ICollection<Commentaire>? Commentaire { get; set; }


        public string IdUtilisateur { get; set; }

        [ForeignKey("IdUtilisateur")]
        public virtual ApplicationUser? ApplicationUser { get; set; }

        public int IdPriorite { get; set; }

        [ForeignKey("IdPriorite")]
        public virtual ParamPriorite? ParamPriorite { get; set; }

        public int IdStatus { get; set; }

        [ForeignKey("IdStatus")]
        public virtual ParamStatus? ParamStatus { get; set; }

        public int IdType { get; set; }

        [ForeignKey("IdType")]
        public virtual ParamType? ParamType { get; set; }

        public int IdProjet { get; set; }

        [ForeignKey("IdProjet")]
        public virtual Projet? Projet { get; set; }

        public int? IdTacheParent { get; set; }

        [ForeignKey("IdTacheParent")]
        public virtual Tache? TacheParent { get; set; }

    }
}
