using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortalVioo.Interface;
using PortalVioo.ModelsApp;

namespace PortalVioo.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PrioriteController(IRepositoryGenericApp<ParamPriorite> repository) : ControllerBase
    {
        private readonly IRepositoryGenericApp<ParamPriorite> _repository = repository;

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

        [HttpGet("ActivePriorite")]
        public IActionResult GetActivePriorite()
        {
            try
            {
                var list = _repository.GetAll(condition: x=>x.IsActive, null);
                return Ok(list);
            }
            catch (Exception ex) { return BadRequest(ex.Message); };


        }



        [HttpGet("GetID")]
        public IActionResult GetById([FromQuery] int id)
        {
            var cl = _repository.Get(id);
            if (cl != null) { return Ok(cl); } else { return NotFound("ParamPriorite Not Found !"); }

        }

        [HttpPost("AddParamPriorite")]
        public IActionResult Ajout([FromBody] ParamPriorite clp)
        {

            var result = _repository.Add(clp);
            if (result != null) { return Ok(result); } else { return BadRequest("Vérifier corp objet !"); }

        }

        [HttpPost("UpdateParamPriorite")]
        public IActionResult Update([FromBody] ParamPriorite clp)
        {

            var result = _repository.Update(clp);
            if (result != null) { return Ok(result); } else { return BadRequest("Vérifier corp objet !"); }

        }


        [HttpPost("ChangerStatus")]
        public IActionResult Changertatus([FromBody] ParamPriorite clp)
        {
            clp.IsActive = ! clp.IsActive;
            var result = _repository.Update(clp);
            if (result != null) { return Ok(result); } else { return BadRequest("Vérifier corp objet !"); }

        }

        [HttpDelete("DeleteParamPriorite")]
        public IActionResult Delete([FromQuery] int id)
        {
            var result = _repository.Delete(id);

            if (result != null) { return Ok(result); } else { return NotFound("ParamPriorite Not Found !"); }
        }
    }
}
  
