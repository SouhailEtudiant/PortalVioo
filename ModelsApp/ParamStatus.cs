using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PortalVioo.ModelsApp
{
    public class ParamStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string LibelleStatus { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Tache>? Tache { get; set; }


    }
}
