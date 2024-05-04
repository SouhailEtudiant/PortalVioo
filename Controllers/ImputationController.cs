using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalVioo.DTO;
using PortalVioo.Interface;
using PortalVioo.ModelsApp;

namespace PortalVioo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImputationController(IRepositoryGenericApp<Imputation> repository, IMapper mapper) : ControllerBase
    {
        private readonly IRepositoryGenericApp<Imputation> _repository = repository;
        private readonly IMapper _mapper = mapper;



        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var list = _repository.GetAll(null, includes: z => z.Include(b => b.ApplicationUser).Include(x => x.Tache));
                var dto = _mapper.Map<List<ImputationDTO>>(list);

                return Ok(dto);
            }
            catch (Exception ex) { return BadRequest(ex.Message); };


        }


        [HttpGet("GetImputationByUser")]
        public IActionResult GetImputationByUser()
        {
            try
            {
                var list = _repository.GetAll(null, includes: z => z.Include(x => x.Tache));
                var dto = _mapper.Map<List<imputationGetDTO>>(list);

                return Ok(dto);
            }
            catch (Exception ex) { return BadRequest(ex.Message); };


        }



        [HttpGet("GetID")]
        public IActionResult GetById([FromQuery] int id)
        {
            var cl = _repository.Get(id);
            if (cl != null) { return Ok(cl); } else { return NotFound("Imputation Not Found !"); }

        }

        [HttpPost("AddImputation")]
        public IActionResult Ajout([FromBody] Imputation clp)
        {
            var result = _repository.Add(clp);
            if (result != null) { return Ok(result); } else { return BadRequest("Vérifier corp objet !"); }

        }

        [HttpPost("UpdateImputation")]
        public IActionResult Update([FromBody] Imputation clp)
        {

            var result = _repository.Update(clp);
            if (result != null) { return Ok(result); } else { return BadRequest("Vérifier corp objet !"); }

        }

        [HttpPost("DeleteImputation")]
        public IActionResult Delete([FromQuery] int id)
        {
            var result = _repository.Delete(id);

            if (result != null) { return Ok(result); } else { return NotFound("Imputation Not Found !"); }
        }
    }
}
