using PortalVioo.Models;
using PortalVioo.ModelsApp;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalVioo.DTO
{
    public class TacheDTO
    {
        public int Id { get; set; }
        public string TacheTitre { get; set; }
        public string TacheDescription { get; set; }

        public decimal ChargeEstime { get; set; }
        public decimal ChargeReele { get; set; }
        public DateOnly DateDebut { get; set; }
        public DateOnly DateFin { get; set; }

        public string IdUtilisateur { get; set; }

        public string username { get; set; }

        public int IdPriorite { get; set; }

       public string prioriteLabel { get; set; }

        public int IdStatus { get; set; }

        public string StatusLabel { get; set; }


        public int IdType { get; set; }

        public string TypeLabel { get; set; }

        public int IdProjet { get; set; }

        public string TitreProjet { get; set; }

        public int IdTacheParent { get; set; }

        
    }
}
