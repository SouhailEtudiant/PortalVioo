using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniProjectBack.ModelsAuth;
using PortalVioo.DTO;
using PortalVioo.Interface;
using PortalVioo.Models;
using PortalVioo.ModelsApp;
using System.Data;

namespace PortalVioo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembreProjetController(IRepositoryGenericApp<MembreProjet> repository ,
        IRepositoryGenericApp<Projet> repositoryProjet, IMapper mapper
        , UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
        IRepositoryGenericApp<ApplicationUser> repositoryUser) : ControllerBase
    {
        private readonly IRepositoryGenericApp<Projet> _repositoryProjet = repositoryProjet;
        private readonly IRepositoryGenericApp<MembreProjet> _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly IRepositoryGenericApp<ApplicationUser> _repositoryUser = repositoryUser;




        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var list = _repository.GetAll(null, null);
                return Ok(list);
            }
            catch (Exception ex) { return BadRequest(ex.Message); };


        }

        [HttpGet("NombreEmployeeParProjet")]
        public IActionResult GetNombreEmployeeParProjet()
        {
          
            var ListProjet = _repositoryProjet.GetAll(null, null);
            List<Dashboard> dashboards = new List<Dashboard>();
            
            foreach (var projet in ListProjet)
            {
                var nombreMembre = _repository.GetAll(condition: x=> x.IdProjet==projet.Id, null).Count();
                Dashboard dash = new Dashboard();
                dash.libelle = projet.ProjetTitre;
                dash.nombre = nombreMembre;
                dashboards.Add(dash); 
            }

            return Ok(dashboards);


        }


        [HttpGet("GetProjetParUser")]
        public IActionResult GetProjetParUser([FromQuery] string userId)
        {
          
                List<Projet> pr = new List<Projet>(); 
                var list = _repository.GetAll(condition :  x=> x.IdUtilisateur==userId, null);
            for (int i = 0; i < list.Count; i++)
            {
                var cl = _repositoryProjet.Get(list[i].IdProjet);
                pr.Add(cl);
            }
          
            return Ok(pr);
            

        }

        private bool checkUserExistInProject (string userId , int projectId)
        {
            var list = _repository.GetAll(condition: x => x.IdUtilisateur == userId && x.IdProjet==projectId, null).FirstOrDefault();

            if (list == null)
                return false;
            else return true; 
        }

        [HttpGet("GetNotAffectedUser")]
        public async Task<IActionResult> GetNotAffectedUser([FromQuery] int projetId)
             
        {
            var listUser = _repositoryUser.GetAll(null, null);
            List<getuserwthrole> listUserWithRole = new List<getuserwthrole>();
            foreach (var user in listUser)
            {
                var role = await _userManager.GetRolesAsync(user);
                IdentityRole roles = roleManager.Roles.Where(x => x.Name.ToLower() == role[0].ToLower()).FirstOrDefault();
                getuserwthrole usrsRole = new getuserwthrole();

                usrsRole.UserId = user.Id;
                usrsRole.RoleId = roles.Id;
                usrsRole.RoleNormalizedName = roles.NormalizedName;
                usrsRole.Username = user.UserName;
                usrsRole.Email = user.Email;
                usrsRole.Role = roles.Name;
                usrsRole.nom = user.NomUser;
                usrsRole.prenom = user.PrenomUser;
                usrsRole.imagePath = user.ImgPath;
                if(! checkUserExistInProject(usrsRole.UserId, projetId) && (usrsRole.RoleNormalizedName != "GESTIONNAIRE" && usrsRole.RoleNormalizedName != "ADMINSTRATEUR"))
                {
                    listUserWithRole.Add(usrsRole);
                }
              


            }
            return Ok(listUserWithRole);


        }



        [HttpGet("GetID")]
        public IActionResult GetById([FromQuery] int id)
        {
            var cl = _repository.Get(id);
            if (cl != null) { return Ok(cl); } else { return NotFound("MembreProjet Not Found !"); }

        }

        [HttpPost("AddMembreProjet")]
        public IActionResult Ajout([FromBody] MembreProjet clp)
        {
           var list = _repository.GetAll(null,null).Where(x=>x.IdProjet == clp.IdProjet && x.IdUtilisateur==clp.IdUtilisateur).ToList(); 
            if (list.Count > 0)
            {
                return BadRequest("Utilisateur exist dans le projet");
            }
            else
            {
                var result = _repository.Add(clp);
                if (result != null) { return Ok(result); } else { return NotFound("Vérifier corp objet !"); }
            }
           

        }

        [HttpPost("UpdateMembreProjet")]
        public IActionResult Update([FromBody] MembreProjet clp)
        {

            var result = _repository.Update(clp);
            if (result != null) { return Ok(result); } else { return BadRequest("Vérifier corp objet !"); }

        }

        [HttpDelete("DeleteMembreProjet")]
        public IActionResult Delete([FromQuery] int idProjet ,[FromQuery] string idUser)
        {
            var list = _repository.GetAll(null, null).Where(x => x.IdProjet == idProjet && x.IdUtilisateur == idUser).FirstOrDefault();
            if ( list!= null)
            {
                var result = _repository.Delete(list.IdMembrePorjet);
                return Ok(result);

            }
            else { return NotFound("MembreProjet Not Found !"); }
        }
    }
}
 
