using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Entities;
using MediatR;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateLED
{
    public class CreateLEDCommandHandler : IRequestHandler<CreateLEDCommand, GenericResponse<LED>>
    {

        private readonly ElectionMaterialManagerContext _db;

        public CreateLEDCommandHandler(ElectionMaterialManagerContext db)
        {
            _db = db;
        }

        public async Task<GenericResponse<LED>> Handle(CreateLEDCommand request, CancellationToken cancellationToken)
        {
            var response = new GenericResponse<LED>() { Success = false };
            try
            {
                var led = new LED
                {
                    Area = request.Area,

                    Latitude = (double)request.Latitude,
                    Longitude = (double)request.Longitude,
                    Priority = (int)request.Priority,
                    Size = request.Size,
                    Cost = (decimal)request.Cost,
                    StatusId = request.StatusId,
                    AuthorId = request.AuthorId,
                    RefreshRate = (int)request.RefreshRate,
                    Resolution = request.Resolution,
                };
                _db.Add(led);
                await _db.SaveChangesAsync();
                response.Success = true;
                response.Data = led;
                response.Message = $"/api/v1/election-item/{led.Id}";
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
