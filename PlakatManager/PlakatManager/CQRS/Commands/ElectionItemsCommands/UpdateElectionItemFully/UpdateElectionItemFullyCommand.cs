using ElectionMaterialManager.CQRS.Responses;
using MediatR;
using System.Text.Json.Serialization;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.UpdateElectionItemFully
{
    public class UpdateElectionItemFullyCommand : IRequest<Response>
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Area { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int Priority { get; set; }

        public string? Size { get; set; }

        public decimal? Cost { get; set; }

        public IEnumerable<int> Tags { get; set; }
        public int StatusId { get; set; }


        public string? PaperType { get; set; }

        public int? RefreshRate { get; set; }

        public string? Resolution { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

    }
}
