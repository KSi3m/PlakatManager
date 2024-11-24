using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using System.ComponentModel;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreatePoster
{
    public class CreatePosterCommand: IRequest<GenericResponse<ElectionItemDto>>
    {
        public string? Area { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? Priority { get; set; }
        public string? Size { get; set; }

        public decimal? Cost { get; set; }

        [DefaultValue(1)]
        public int StatusId { get; set; }
        public IEnumerable<int> Tags { get; set; }

        public int AuthorId { get; set; }

        public string? PaperType { get; set; }


    }
}
