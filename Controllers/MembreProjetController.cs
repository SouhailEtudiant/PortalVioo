using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        , UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager ) : ControllerBase
    {
        private readonly IRepositoryGenericApp<Projet> _repositoryProjet = repositoryProjet;
        private readonly IRepositoryGenericApp<MembreProjet> _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

 

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

        [HttpPost("DeleteMembreProjet")]
        public IActionResult Delete([FromQuery] int id)
        {
            var result = _repository.Delete(id);

            if (result != null) { return Ok(result); } else { return NotFound("MembreProjet Not Found !"); }
        }
    }
}
 
