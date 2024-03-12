using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PortalVioo.ModelsApp
{
    public class Projet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ProjetTitre { get; set; }
        public string ProjetDescription { get; set; }
        public string ProjetImage { get; set; }
        public virtual ICollection<MembreProjet>? MembreProjet { get; set; }

    }
}
