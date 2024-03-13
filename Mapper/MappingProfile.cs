using AutoMapper;
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


        }
    }

}

