using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MiniProjectBack.ModelsAuth;
using PortalVioo.DTO;
using PortalVioo.Interface;
using PortalVioo.Models;
using PortalVioo.ModelsApp;

namespace PortalVioo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TacheController(IRepositoryGenericApp<Tache> repository,
        IRepositoryGenericApp<Projet> repositoryProjet,
        IRepositoryGenericApp<ParamStatus> repositorystatus , IMapper mapper,
         IRepositoryGenericApp<MembreProjet> repositoryMembreProjet) : ControllerBase
    {
        private readonly IRepositoryGenericApp<Tache> _repository = repository;
        private readonly IRepositoryGenericApp<Projet> _repositoryProjet = repositoryProjet;
        private readonly IRepositoryGenericApp<ParamStatus> _repositorystatus = repositorystatus;
        private readonly IRepositoryGenericApp<MembreProjet> _repositoryMembreProjet = repositoryMembreProjet;
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

        [HttpGet]
        [Route("GetNombreOfTasks")]
        public IActionResult GetNombreOfTasks([FromQuery]string userId)
        {
            List<NumberUsers> ListNumbers = new List<NumberUsers>();
            for (int i = 0; i < 4; i++)
            {
                NumberUsers nb = new NumberUsers() { Id = i, nom = "", nombre = 0 };
                ListNumbers.Add(nb);
            }

            var ListTache = _repository.GetAll(condition: x=> x.IdUtilisateur==userId, null).ToList();

            ListNumbers[0].nom = "Nombre Total des Taches";
            ListNumbers[1].nom = "Nombre Total des Bogues";
            ListNumbers[2].nom = "Nombre des Taches en cours";
            ListNumbers[3].nom = "Nombre des Taches Terminé";

            foreach (var Tache in ListTache)
            {
                ListNumbers[0].nombre++;
                
                if (Tache.IdStatus == 6)
                {
                    ListNumbers[1].nombre++;
                }

                else if (Tache.IdStatus == 3)
                    ListNumbers[2].nombre++;
                else if (Tache.IdStatus == 4)
                  ListNumbers[3].nombre++;

            }


            return Ok(ListNumbers);
        }


        [HttpGet("dashboardMultiLine")]
        public IActionResult dashboardMultiLine()
        {

            var ListProjet = _repositoryProjet.GetAll(null, null);
            List<dashboardMultipleLine> dashboards = new List<dashboardMultipleLine>();
                    foreach (var proj in ListProjet)
                    {
                        var tacheEnCours = _repository.GetAll(condition: x => x.IdStatus == 3 && x.IdProjet==proj.Id, null).Count();
                        var tacheTermine = _repository.GetAll(condition: x => x.IdStatus == 4 && x.IdProjet == proj.Id, null).Count();
                        var tacheBogue = _repository.GetAll(condition: x => x.IdStatus == 6 && x.IdProjet == proj.Id, null).Count();

                         dashboardMultipleLine dash = new dashboardMultipleLine();
                        dash.libelle = proj.ProjetTitre;
                        dash.tacheEnCours = tacheEnCours;
                         dash.tacheTermine = tacheTermine;
                         dash.tacheBug = tacheBogue;
                        dashboards.Add(dash);
                    }  

            return Ok(dashboards);


        }

        [HttpGet("dashboardMultiLineUser")]
        public IActionResult dashboardMultiLineUser([FromQuery] string userId)
        {
          
            var ListProjetInMembre = _repositoryMembreProjet.GetAll(condition: x =>x.IdUtilisateur==userId, null);
            List<Projet> ListProjet = new List<Projet>();
            foreach (var  membre in ListProjetInMembre)
            {
                var projet = _repositoryProjet.Get(membre.IdProjet);
                ListProjet.Add(projet); 


            }
            List<dashboardMultipleLine> dashboards = new List<dashboardMultipleLine>();
            foreach (var proj in ListProjet)
            {
                var tacheEnCours = _repository.GetAll(condition: x => x.IdStatus == 3 && x.IdProjet == proj.Id && x.IdUtilisateur==userId , null).Count();
                var tacheTermine = _repository.GetAll(condition: x => x.IdStatus == 4 && x.IdProjet == proj.Id && x.IdUtilisateur == userId, null).Count();
                var tacheBogue = _repository.GetAll(condition: x => x.IdStatus == 6 && x.IdProjet == proj.Id && x.IdUtilisateur == userId, null).Count();

                dashboardMultipleLine dash = new dashboardMultipleLine();
                dash.libelle = proj.ProjetTitre;
                dash.tacheEnCours = tacheEnCours;
                dash.tacheTermine = tacheTermine;
                dash.tacheBug = tacheBogue;
                dashboards.Add(dash);
            }

            return Ok(dashboards);


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
        public IActionResult GetListOfListsParProjet([FromQuery]int projectID, [FromQuery] string? userId)
        {
            List<TacheListcs> tl = new List<TacheListcs>();

            if(userId==null)
            {
                var listStatus = _repositorystatus.GetAll(null, null);
                for (int i = 0; i < listStatus.Count; i++)

                {
                    var list = _repository.GetAll(condition: x => x.IdStatus == listStatus[i].Id && x.IdProjet == projectID
                    && x.IdTacheParent == null
                    , includes: z => z.Include(b => b.ApplicationUser).Include(x => x.Projet)
                    .Include(w => w.ParamStatus).Include(a => a.ParamPriorite).Include(e => e.ParamType));
                    var dto = _mapper.Map<List<TacheDTO>>(list);
                   foreach (var item in dto)
                    {
                        if (item.DateFin > DateOnly.FromDateTime(DateTime.Now))
                        {
                            item.retard = "1";
                        }
                        else if (item.DateFin < DateOnly.FromDateTime(DateTime.Now))
                            item.retard = "-1";
                        else item.retard = "0"; 
                    }
                    var tache = new TacheListcs { idStatus = listStatus[i].Id, labelStatus = listStatus[i].LibelleStatus, listTache = dto, nombreTache = dto.Count };

                    tl.Add(tache);
                }
            }
            else
            {
                var listStatus = _repositorystatus.GetAll(null, null);
                for (int i = 0; i < listStatus.Count; i++)

                {
                    var list = _repository.GetAll(condition: x => x.IdStatus == listStatus[i].Id && x.IdProjet == projectID
                    && x.IdTacheParent == null && x.IdUtilisateur==userId
                    , includes: z => z.Include(b => b.ApplicationUser).Include(x => x.Projet)
                    .Include(w => w.ParamStatus).Include(a => a.ParamPriorite).Include(e => e.ParamType));
                    var dto = _mapper.Map<List<TacheDTO>>(list);
                    foreach (var item in dto)
                    {
                        if (item.DateFin > DateOnly.FromDateTime(DateTime.Now))
                        {
                            item.retard = "1";
                        }
                        else if (item.DateFin < DateOnly.FromDateTime(DateTime.Now))
                            item.retard = "-1";
                        else item.retard = "0";
                    }
                    var tache = new TacheListcs { idStatus = listStatus[i].Id, labelStatus = listStatus[i].LibelleStatus, listTache = dto, nombreTache = dto.Count };

                    tl.Add(tache);
                }
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

