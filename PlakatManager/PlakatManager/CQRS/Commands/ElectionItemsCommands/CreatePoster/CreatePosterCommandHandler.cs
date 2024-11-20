using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Entities;
using MediatR;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreatePoster
{
    public class CreatePosterCommandHandler : IRequestHandler<CreatePosterCommand, GenericResponse<Poster>>
    {
        private readonly ElectionMaterialManagerContext _db;

        public CreatePosterCommandHandler(ElectionMaterialManagerContext db)
        {
            _db = db;
        }

        public async Task<GenericResponse<Poster>> Handle(CreatePosterCommand request, CancellationToken cancellationToken)
        {
            var response = new GenericResponse<Poster>() { Success = false };
            try
            {
                var poster = new Poster
                {
                    Area = request.Area,

                    Latitude = (double)request.Latitude,
                    Longitude = (double)request.Longitude,
                    Priority = (int)request.Priority,
                    Size = request.Size,
                    Cost = (decimal)request.Cost,
                    StatusId = request.StatusId,
                    AuthorId = request.AuthorId,
                    PaperType = request.PaperType
                };
                _db.Add(poster);
                await _db.SaveChangesAsync();
                response.Success = true;
                response.Data = poster;
                response.Message = $"/api/v1/election-item/{poster.Id}";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
