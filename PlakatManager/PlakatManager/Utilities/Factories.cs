using AutoMapper;
using PlakatManager.Dtos;
using PlakatManager.Entities;
using System.Text.Json;

namespace PlakatManager.Utilities
{
    public class BillboardFactory : IElectionItemFactory
    {
        private readonly IMapper _mapper;

        public BillboardFactory(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ElectionItem Create(ElectionItemRequestDTO dto)
        {
            return _mapper.Map<Billboard>(dto);
        }
    }

    public class PosterFactory : IElectionItemFactory
    {
        private readonly IMapper _mapper;

        public PosterFactory(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ElectionItem Create(ElectionItemRequestDTO dto)
        {
            return _mapper.Map<Poster>(dto);
        }
    }

    public class LEDFactory : IElectionItemFactory
    {
        private readonly IMapper _mapper;

        public LEDFactory(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ElectionItem Create(ElectionItemRequestDTO dto)
        {
            return _mapper.Map<LED>(dto);
        }
    }
}
