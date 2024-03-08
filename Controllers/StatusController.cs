using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortalVioo.Interface;
using PortalVioo.ModelsApp;

namespace PortalVioo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController(IRepositoryGenericApp<ParamStatus> repository) : ControllerBase 
    {
        private readonly IRepositoryGenericApp<ParamStatus> _repository = repository;

        [HttpGet]
        public IActionResult Get()
        {
            var list = _repository.GetAll(null, null);

            return Ok(list);
        }

        [HttpGet("GetID")]
        public IActionResult GetById([FromQuery] int id)
        {
            var cl = _repository.Get(id);
            if (cl != null) { return Ok(cl); } else { return NotFound("ParamStatus Not Found !"); }

        }

        [HttpPost("AddParamStatus")]
        public IActionResult Ajout([FromBody] ParamStatus clp)
        {

            var result = _repository.Add(clp);
            if (result != null) { return Ok(result); } else { return BadRequest("Vérifier corp objet !"); }

        }

        [HttpPost("UpdateParamStatus")]
        public IActionResult Update([FromBody] ParamStatus clp)
        {

            var result = _repository.Update(clp);
            if (result != null) { return Ok(result); } else { return BadRequest("Vérifier corp objet !"); }

        }

        [HttpPost("DeleteParamStatus")]
        public IActionResult Delete([FromQuery] int id)
        {
            var result = _repository.Delete(id);

            if (result != null) { return Ok(result); } else { return NotFound("ParamStatus Not Found !"); }
        }
    }
}

