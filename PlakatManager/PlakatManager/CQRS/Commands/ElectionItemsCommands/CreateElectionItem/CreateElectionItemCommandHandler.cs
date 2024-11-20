using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateBillboard;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Entities;
using ElectionMaterialManager.Utilities;
using MediatR;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateElectionItem
{
    public class CreateElectionItemCommandHandler : IRequestHandler<CreateElectionItemCommand, GenericResponse<ElectionItem>>
    {

        private readonly ElectionMaterialManagerContext _db;
        private readonly ElectionItemFactoryRegistry _factoryRegistry;

        public CreateElectionItemCommandHandler(ElectionMaterialManagerContext db, ElectionItemFactoryRegistry factoryRegistry)
        {
            _db = db;
            _factoryRegistry = factoryRegistry;
        }

        public async Task<GenericResponse<ElectionItem>> Handle(CreateElectionItemCommand request, CancellationToken cancellationToken)
        {
            var response = new GenericResponse<ElectionItem>() { Success = false };
            try
            {
                var type = request.Type;
                var electionItem = _factoryRegistry.CreateElectionItem(type, request);

                _db.Add(electionItem);
                await _db.SaveChangesAsync();

                response.Success = true;
                response.Data = electionItem;
                response.Message = $"/api/v1/election-item/{electionItem.Id}";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;

        }
    }
}
