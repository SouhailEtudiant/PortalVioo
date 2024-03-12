using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortalVioo.Interface;
using PortalVioo.ModelsApp;

namespace PortalVioo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembreProjetController(IRepositoryGenericApp<MembreProjet> repository) : ControllerBase
    {
        private readonly IRepositoryGenericApp<MembreProjet> _repository = repository;

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

            var result = _repository.Add(clp);
            if (result != null) { return Ok(result); } else { return BadRequest("Vérifier corp objet !"); }

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
 
