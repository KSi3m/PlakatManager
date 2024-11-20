using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateLED;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Entities;
using MediatR;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateBillboard
{
    public class CreateBillboardCommandHandler: IRequestHandler<CreateBillboardCommand, GenericResponse<Billboard>>
    {
        private readonly ElectionMaterialManagerContext _db;

        public CreateBillboardCommandHandler(ElectionMaterialManagerContext db)
        {
            _db = db;
        }

        public async Task<GenericResponse<Billboard>> Handle(CreateBillboardCommand request, CancellationToken cancellationToken)
        {
            var response = new GenericResponse<Billboard>() { Success = false };
            try
            {
                var billboard = new Billboard
                {
                    Area = request.Area,

                    Latitude = (double)request.Latitude,
                    Longitude = (double)request.Longitude,
                    Priority = (int)request.Priority,
                    Size = request.Size,
                    Cost = (decimal)request.Cost,
                    StatusId = request.StatusId,
                    AuthorId = request.AuthorId,
                    StartDate = (DateTime)request.StartDate,
                    EndDate = (DateTime)request.EndDate
                };
                _db.Add(billboard);
                await _db.SaveChangesAsync();
                response.Success = true;
                response.Data = billboard;
                response.Message = $"/api/v1/election-item/{billboard.Id}";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
