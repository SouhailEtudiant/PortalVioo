using PortalVioo.Models;
using PortalVioo.ModelsApp;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PortalVioo.DTO
{
    public class ImputationDTO
    {
        public int Id { get; set; }

        public DateOnly date { get; set; }

        public decimal chargeEnHeure { get; set; }

        public bool IsActive { get; set; }

        public string IdUtilisateur { get; set; }

       public string username { get; set; }

        
        public int IdTache { get; set; }
        public string TacheTitre { get; set; }
    }
}
