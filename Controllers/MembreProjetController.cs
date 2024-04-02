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
    public class MembreProjetController(IRepositoryGenericApp<MembreProjet> repository , IMapper mapper
        , UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager ) : ControllerBase
    {
        private readonly IRepositoryGenericApp<MembreProjet> _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;



        //[HttpGet]
        //public async Task<IActionResult> GetAsync()
        //{
        //    try
        //    {
        //        var list = _repository.GetAll(null, includes: z => z.Include(b => b.ApplicationUser).Include(x => x.Projet));
        //        var dto = _mapper.Map<List<MembreProjetDTO>>(list);
        //        for ( var i = 0;i< dto.Count; i++)
        //        {
        //            var user = await userManager.FindByNameAsync(dto[i].username);
        //            var userRoles = await userManager.GetRolesAsync(user);
        //            IdentityRole roles = roleManager.Roles.Where(x => x.Name.ToLower() == userRoles[0].ToLower()).FirstOrDefault();
        //            dto[i].roleMembre = roles.Name;
        //        }
        //        return Ok(dto);
        //    }
        //    catch (Exception ex) { return BadRequest(ex.Message); };


        //}

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
 
