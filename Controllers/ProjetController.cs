﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortalVioo.Interface;
using PortalVioo.ModelsApp;

namespace PortalVioo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjetController(IRepositoryGenericApp<Projet> repository) : ControllerBase
    {
        private readonly IRepositoryGenericApp<Projet> _repository = repository;

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
            if (cl != null) { return Ok(cl); } else { return NotFound("Projet Not Found !"); }

        }

        [HttpPost("AddProjet")]
        public IActionResult Ajout([FromBody] Projet clp)
        {

            var result = _repository.Add(clp);
            if (result != null) { return Ok(result); } else { return BadRequest("Vérifier corp objet !"); }

        }

        [HttpPost("UpdateProjet")]
        public IActionResult Update([FromBody] Projet clp)
        {

            var result = _repository.Update(clp);
            if (result != null) { return Ok(result); } else { return BadRequest("Vérifier corp objet !"); }

        }

        [HttpDelete("DeleteProjet")]
        public IActionResult Delete([FromQuery] int id)
        {
            var result = _repository.Delete(id);

            if (result != null) { return Ok(result); } else { return NotFound("Projet Not Found !"); }
        }
    }
}
 
