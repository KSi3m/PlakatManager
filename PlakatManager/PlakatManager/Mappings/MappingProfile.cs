using AutoMapper;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;

namespace ElectionMaterialManager.Mappings
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
