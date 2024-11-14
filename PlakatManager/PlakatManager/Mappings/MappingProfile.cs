using AutoMapper;
using PlakatManager.Dtos;
using PlakatManager.Entities;

namespace PlakatManager.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ElectionItemRequestDTO, LED>();
            CreateMap<ElectionItemRequestDTO, Poster>();
            CreateMap<ElectionItemRequestDTO, Billboard>();
        }
    }
}
