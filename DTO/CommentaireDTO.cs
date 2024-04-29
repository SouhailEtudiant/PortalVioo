using PortalVioo.Models;
using PortalVioo.ModelsApp;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalVioo.DTO
{
    public class CommentaireDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateOnly Date { get; set; }
        public bool IsActive { get; set; }
        public string CreePar { get; set; }
        public string username { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public string userImage { get; set; }
        public int IdTache { get; set; }
        public string TacheTitle { get; set; }

    }
}
