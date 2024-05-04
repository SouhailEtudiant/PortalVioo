using PortalVioo.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PortalVioo.ModelsApp
{
    [Keyless]
    public class MembreProjetDTO
    {
      
        public int IdMembrePorjet { get; set; }
        public string IdUtilisateur { get; set; }
        public int IdProjet { get; set; }
        public string username { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public string imgpath { get; set; }
        public string TitreProjet { get; set; }
        public string roleMembre { get; set; }

    }
}
