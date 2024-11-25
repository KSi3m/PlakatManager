using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.EditElectionItem
{
    public class EditElectionItemCommand: IRequest<Response>
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string? Area { get; set; }

        public double? Latitude { get; set; }
 
        public double? Longitude { get; set; }

        public int? Priority { get; set; }
      
        public string? Size { get; set; }
      
        public decimal? Cost { get; set; }

        public IEnumerable<int>? Tags { get; set; }
        public int? StatusId { get; set; }
       
        public int? AuthorId { get; set; }
      
        public string? PaperType { get; set; }
      
        public int? RefreshRate { get; set; }
      
        public string? Resolution { get; set; }
       
        public DateTime? StartDate { get; set; }
  
        public DateTime? EndDate { get; set; }
    }
}
