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
    public class TacheController(IRepositoryGenericApp<Tache> repository, IRepositoryGenericApp<ParamStatus> repositorystatus , IMapper mapper) : ControllerBase
    {
        private readonly IRepositoryGenericApp<Tache> _repository = repository;
        private readonly IRepositoryGenericApp<ParamStatus> _repositorystatus = repositorystatus;
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

        [HttpGet("GetListOfLists")]
        public IActionResult GetListOfLists()
        {
            List<TacheListcs> tl = new List<TacheListcs>();
          

            var listStatus = _repositorystatus.GetAll(null, null);
            for (int i = 0; i < listStatus.Count; i++)

            {
                var list = _repository.GetAll(condition: x => x.IdStatus == listStatus[i].Id && x.IdTacheParent==null, includes: z => z.Include(b => b.ApplicationUser).Include(x => x.Projet)
                .Include(w => w.ParamStatus).Include(a => a.ParamPriorite).Include(e => e.ParamType));
                var dto = _mapper.Map<List<TacheDTO>>(list);
                var tache = new TacheListcs { idStatus = listStatus[i].Id, labelStatus = listStatus[i].LibelleStatus, listTache = dto ,nombreTache = dto.Count };

                tl.Add(tache);
            }
            return Ok(tl);


        }


        [HttpGet("GetListOfListsParProjet")]
        public IActionResult GetListOfListsParProjet([FromQuery]int projectID)
        {
            List<TacheListcs> tl = new List<TacheListcs>();


            var listStatus = _repositorystatus.GetAll(null, null);
            for (int i = 0; i < listStatus.Count; i++)

            {
                var list = _repository.GetAll(condition: x => x.IdStatus == listStatus[i].Id && x.IdProjet == projectID
                && x.IdTacheParent == null
                , includes: z => z.Include(b => b.ApplicationUser).Include(x => x.Projet)
                .Include(w => w.ParamStatus).Include(a => a.ParamPriorite).Include(e => e.ParamType));
                var dto = _mapper.Map<List<TacheDTO>>(list);
                var tache = new TacheListcs { idStatus = listStatus[i].Id, labelStatus = listStatus[i].LibelleStatus, listTache = dto, nombreTache = dto.Count };

                tl.Add(tache);
            }
            return Ok(tl);


        }

        [HttpGet("GetSousTache")]
        public IActionResult GetSousTache([FromQuery] int idparent)
        {
            try
            {
                var list = _repository.GetAll(condition: x => x.IdTacheParent == idparent, includes: z => z.Include(b => b.ApplicationUser).Include(x => x.Projet)
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
            var list = _repository.GetAll(condition : x=> x.Id==id, includes: z => z.Include(b => b.ApplicationUser).Include(x => x.Projet)
                 .Include(w => w.ParamStatus).Include(a => a.ParamPriorite).Include(e => e.ParamType));
            var dto = _mapper.Map<List<TacheDTO>>(list);
            if (dto != null) { return Ok(dto); } else { return NotFound("Tache Not Found !"); }

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
        [HttpPost("UpdateTachebyId")]
        public IActionResult UpdateTachebyId([FromBody] UpdateTaskStatus ts)
        {
            var task = _repository.Get(ts.tacheId);
            task.IdStatus = ts.idStatus ;
            var result = _repository.Update(task);
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

