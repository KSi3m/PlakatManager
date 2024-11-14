using AutoMapper;
using PlakatManager.Dtos;
using PlakatManager.Entities;
using System.Text.Json;

namespace PlakatManager.Utilities
{
    public interface IElectionItemFactory
    {
        ElectionItem Create(ElectionItemRequestDTO dto);
    }

    public abstract class ElectionItemFactory<T>: IElectionItemFactory where T: ElectionItem
    {
        protected readonly IMapper _mapper;

        protected ElectionItemFactory(IMapper mapper)
        {
            _mapper = mapper;
        }

        public virtual ElectionItem Create(ElectionItemRequestDTO dto)
        {
            return _mapper.Map<T>(dto);
        }
    }
}
