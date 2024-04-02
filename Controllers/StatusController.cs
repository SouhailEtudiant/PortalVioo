using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortalVioo.Interface;
using PortalVioo.ModelsApp;
using System.Collections.Generic;
using System.Linq.Expressions;

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
            try
            {
                var list = _repository.GetAll(null, null);
                return Ok(list);
            }
            catch (Exception ex) { return BadRequest(ex.Message); };
         
           
        }

        [HttpPost("ChangerStatus")]
        public IActionResult Changertatus([FromBody] ParamStatus clp)
        {
            clp.IsActive = !clp.IsActive;
            var result = _repository.Update(clp);
            if (result != null) { return Ok(result); } else { return BadRequest("Vérifier corp objet !"); }

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

        [HttpDelete("DeleteParamStatus")]
        public IActionResult Delete([FromQuery] int id)
        {
            var result = _repository.Delete(id);

            if (result != null) { return Ok(result); } else { return NotFound("ParamStatus Not Found !"); }
        }
    }
}

