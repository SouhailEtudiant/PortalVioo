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
             .ReverseMap();

            CreateMap<Tache, TacheDTO>().ForMember(d => d.username, i => i.MapFrom(src => src.ApplicationUser.UserName))
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


        }
    }

}

