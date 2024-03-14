using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalVioo.DTO;
using PortalVioo.Interface;
using PortalVioo.Models;
using PortalVioo.ModelsApp;

namespace PortalVioo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TacheController(IRepositoryGenericApp<Tache> repository, IMapper mapper) : ControllerBase
    {
        private readonly IRepositoryGenericApp<Tache> _repository = repository;
        private readonly IMapper _mapper = mapper;



        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var list = _repository.GetAll(null, includes: z => z.Include(b => b.ApplicationUser).Include(x => x.Projet)
                .Include(w=> w.ParamStatus).Include(a=>a.ParamPriorite).Include(e=>e.ParamType));
                var dto = _mapper.Map<List<TacheDTO>>(list);
                
                return Ok(dto);
            }
            catch (Exception ex) { return BadRequest(ex.Message); };


        }

        [HttpGet("GetSousTache")]
        public IActionResult GetSousTache()
        {
            try
            {
                var list = _repository.GetAll(condition: x => x.IdTacheParent != null , includes: z => z.Include(b => b.ApplicationUser).Include(x => x.Projet)
                .Include(w => w.ParamStatus).Include(a => a.ParamPriorite).Include(e => e.ParamType));
                var dto = _mapper.Map<List<TacheDTO>>(list);

                return Ok(dto);
            }
            catch (Exception ex) { return BadRequest(ex.Message); };


        }

        [HttpGet("GetTacheParentList")]
        public IActionResult GetTacheParent()
        {
            try
            {
                var list = _repository.GetAll(condition: x => x.IdTacheParent == null, includes: z => z.Include(b => b.ApplicationUser).Include(x => x.Projet)
                .Include(w => w.ParamStatus).Include(a => a.ParamPriorite).Include(e => e.ParamType));
                var dto = _mapper.Map<List<TacheDTO>>(list);

                return Ok(dto);
            }
            catch (Exception ex) { return BadRequest(ex.Message); };


        }





        [HttpGet("GetID")]
        public IActionResult GetById([FromQuery] int id)
        {
            var cl = _repository.Get(id);
            if (cl != null) { return Ok(cl); } else { return NotFound("Tache Not Found !"); }

        }

        [HttpPost("AddTache")]
        public IActionResult Ajout([FromBody] Tache clp)
        {

            var result = _repository.Add(clp);
            if (result != null) { return Ok(result); } else { return BadRequest("Vérifier corp objet !"); }

        }

        [HttpPost("UpdateTache")]
        public IActionResult Update([FromBody] Tache clp)
        {

            var result = _repository.Update(clp);
            if (result != null) { return Ok(result); } else { return BadRequest("Vérifier corp objet !"); }

        }

        [HttpPost("DeleteTache")]
        public IActionResult Delete([FromQuery] int id)
        {
            var result = _repository.Delete(id);

            if (result != null) { return Ok(result); } else { return NotFound("Tache Not Found !"); }
        }
    }

}

