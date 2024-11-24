using AutoMapper;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateElectionItem;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using System.Text.Json;

namespace ElectionMaterialManager.Utilities
{
    public interface IElectionItemFactory
    {
        ElectionItem Create(CreateElectionItemCommand dto);
    }

    public abstract class ElectionItemFactory<T>: IElectionItemFactory where T: ElectionItem
    {
        protected readonly IMapper _mapper;

        protected ElectionItemFactory(IMapper mapper)
        {
            _mapper = mapper;
        }

        public virtual ElectionItem Create(CreateElectionItemCommand command)
        {
            return _mapper.Map<T>(command);
        }
    }
}
