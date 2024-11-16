using AutoMapper;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using System.Text.Json;

namespace ElectionMaterialManager.Utilities
{
    public class BillboardFactory : ElectionItemFactory<Billboard>
    {
        public BillboardFactory(IMapper mapper) : base(mapper)
        {
        }
    }

    public class PosterFactory : ElectionItemFactory<Poster>
    {
        public PosterFactory(IMapper mapper) : base(mapper)
        {
        }
    }

    public class LEDFactory : ElectionItemFactory<LED>
    {
        public LEDFactory(IMapper mapper) : base(mapper)
        {
        }
    }
}
