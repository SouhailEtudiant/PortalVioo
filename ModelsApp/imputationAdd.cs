using PortalVioo.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PortalVioo.ModelsApp
{
    public class imputationAdd
    {
     //   public int Id { get; set; }

        public string date { get; set; }

        public decimal chargeEnHeure { get; set; }

        public bool IsActive { get; set; }

        public string IdUtilisateur { get; set; }

        public int IdTache { get; set; }

    }
}
