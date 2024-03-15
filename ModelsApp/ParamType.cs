using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PortalVioo.ModelsApp
{
    public class ParamType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string LibelleType { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Tache>? Tache { get; set; }


    }
}
