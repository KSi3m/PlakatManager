using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using System.ComponentModel;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateElectionItem
{
    public class CreateElectionItemCommand: IRequest<GenericResponse<ElectionItemDto>>
    {
        public string Type { get; set; }
        public LocationDto Location { get; set; }
        public int Priority { get; set; }
        public string? Size { get; set; }

        public decimal? Cost { get; set; }

        public IEnumerable<int>? Tags { get; set; }
        public int StatusId { get; set; }

        public string? PaperType { get; set; }
        public int? RefreshRate { get; set; }
        public string? Resolution { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
