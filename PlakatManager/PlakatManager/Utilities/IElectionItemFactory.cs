using PlakatManager.Dtos;
using PlakatManager.Entities;
using System.Text.Json;

namespace PlakatManager.Utilities
{
    public interface IElectionItemFactory
    {
        ElectionItem Create(ElectionItemRequestDTO dto);
    }
}
