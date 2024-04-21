using PortalVioo.DTO;

namespace PortalVioo.ModelsApp
{
    public class TacheListcs
    {
       public int idStatus {  get; set; }
        public string labelStatus { get; set; }
        public int nombreTache { get; set; }

        public List<TacheDTO> listTache { get; set;}



    }
}
