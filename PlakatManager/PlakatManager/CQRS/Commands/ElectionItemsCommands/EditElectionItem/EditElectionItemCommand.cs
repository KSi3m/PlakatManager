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
        //[DefaultValue(null)]
        public string? Area { get; set; }


        public double? Latitude { get; set; }
       // [DefaultValue(null)]
        public double? Longitude { get; set; }
       // [DefaultValue(null)]
        public int? Priority { get; set; }
       // [DefaultValue(null)]
        public string? Size { get; set; }
      //  [DefaultValue(null)]
        public decimal? Cost { get; set; }
      //  [DefaultValue(null)]
        public int? StatusId { get; set; }
       // [DefaultValue(null)]
        public int? AuthorId { get; set; }
       // [DefaultValue(null)]
        public string? PaperType { get; set; }
       // [DefaultValue(null)]
        public int? RefreshRate { get; set; }
       // [DefaultValue(null)]
        public string? Resolution { get; set; }
       // [DefaultValue(null)]
        public DateTime? StartDate { get; set; }
       // [DefaultValue(null)]
        public DateTime? EndDate { get; set; }
    }
}
