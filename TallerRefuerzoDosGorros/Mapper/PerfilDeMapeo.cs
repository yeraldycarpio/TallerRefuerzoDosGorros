using AutoMapper;
using Gorros.DTOs.GorrosDTOS;
using TallerRefuerzoDosGorros.Models.EN;

namespace TallerRefuerzoDosGorros.Mapper
{
    public class PerfilDeMapeo: Profile
    {
        public PerfilDeMapeo()
        {
            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => src.Nombre));

            // Mapear entre entidad y DTOs
            CreateMap<Gorro, SearchResultGorrosDTO.GorroDTO>();
            CreateMap<CreateGorrosDTO, Gorro>();
            CreateMap<EditGorrosDTO, Gorro>();
            CreateMap<Gorro, GetIdResultGorrosDTO>();
        }
    }
}
