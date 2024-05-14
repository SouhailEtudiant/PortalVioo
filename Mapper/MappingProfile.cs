using AutoMapper;
using PortalVioo.DTO;
using PortalVioo.ModelsApp;
using System.Text.RegularExpressions;

namespace PortalVioo.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MembreProjet, MembreProjetDTO>().ForMember(d => d.username, i => i.MapFrom(src => src.ApplicationUser.UserName))
            .ForMember(d => d.TitreProjet, i => i.MapFrom(src => src.Projet.ProjetTitre))
             .ForMember(d => d.IdProjet, i => i.MapFrom(src => src.Projet.Id))
             .ForMember(d => d.IdUtilisateur, i => i.MapFrom(src => src.ApplicationUser.Id))

             .ForMember(d => d.nom, i => i.MapFrom(src => src.ApplicationUser.NomUser))
             .ForMember(d => d.prenom, i => i.MapFrom(src => src.ApplicationUser.PrenomUser))
             .ForMember(d => d.imgpath, i => i.MapFrom(src => src.ApplicationUser.ImgPath))
             .ReverseMap();

            CreateMap<Tache, TacheDTO>().ForMember(d => d.username, i => i.MapFrom(src => src.ApplicationUser.UserName))
                .ForMember(d => d.nom, i => i.MapFrom(src => src.ApplicationUser.NomUser))
                .ForMember(d => d.prenom, i => i.MapFrom(src => src.ApplicationUser.PrenomUser))
           .ForMember(d => d.TitreProjet, i => i.MapFrom(src => src.Projet.ProjetTitre))
            .ForMember(d => d.IdProjet, i => i.MapFrom(src => src.Projet.Id))
            .ForMember(d => d.IdUtilisateur, i => i.MapFrom(src => src.ApplicationUser.Id))
            .ForMember(d => d.StatusLabel, i => i.MapFrom(src => src.ParamStatus.LibelleStatus))
            .ForMember(d => d.IdStatus, i => i.MapFrom(src => src.ParamStatus.Id))
            .ForMember(d => d.TypeLabel, i => i.MapFrom(src => src.ParamType.LibelleType))
            .ForMember(d => d.IdType, i => i.MapFrom(src => src.ParamType.Id))
            .ForMember(d => d.prioriteLabel, i => i.MapFrom(src => src.ParamPriorite.LibellePriorite))
            .ForMember(d => d.IdPriorite, i => i.MapFrom(src => src.ParamPriorite.Id))
            .ReverseMap();

            CreateMap<Imputation, ImputationDTO>().ForMember(d => d.username, i => i.MapFrom(src => src.ApplicationUser.UserName))
            .ForMember(d => d.TacheTitre, i => i.MapFrom(src => src.Tache.TacheTitre))
             .ForMember(d => d.IdTache, i => i.MapFrom(src => src.Tache.Id))
             .ForMember(d => d.IdUtilisateur, i => i.MapFrom(src => src.ApplicationUser.Id))
             .ReverseMap();

            CreateMap<Imputation, imputationGetDTO>()
                .ForMember(d => d.id, i => i.MapFrom(src => src.Id))
           .ForMember(d => d.title, i => i.MapFrom(src => src.Tache.TacheTitre))
            .ForMember(d => d.start, i => i.MapFrom(src => src.date))
             .ForMember(d => d.prioriteId, i => i.MapFrom(src => src.Tache.IdPriorite))
            .ReverseMap();

            CreateMap<Commentaire, CommentaireDTO>().ForMember(d => d.username, i => i.MapFrom(src => src.ApplicationUser.UserName))
           .ForMember(d => d.TacheTitle, i => i.MapFrom(src => src.Tache.TacheTitre))
            .ForMember(d => d.IdTache, i => i.MapFrom(src => src.Tache.Id))
            .ForMember(d => d.CreePar, i => i.MapFrom(src => src.ApplicationUser.Id))
              .ForMember(d => d.nom, i => i.MapFrom(src => src.ApplicationUser.NomUser))
                .ForMember(d => d.prenom, i => i.MapFrom(src => src.ApplicationUser.PrenomUser))
                  .ForMember(d => d.userImage, i => i.MapFrom(src => src.ApplicationUser.ImgPath))
            .ReverseMap();


        }
    }

}

