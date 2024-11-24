using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ElectionMaterialManager.CQRS.Responses;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.EditElectionItem
{
    public class EditElectionItemCommandHandler: IRequestHandler<EditElectionItemCommand,Response>
    {
        
        private readonly ElectionMaterialManagerContext _db;
        private readonly IMapper _mapper;

        public EditElectionItemCommandHandler(ElectionMaterialManagerContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<Response> Handle(EditElectionItemCommand request, CancellationToken cancellationToken)
        {
            var response = new Response() { Success = false };
            try
            {
                var item = await _db.ElectionItems.FirstOrDefaultAsync(x => x.Id == request.Id);
                if (item == null)
                {
                    response.Message = "Item not found";
                    return response;
                }
                _mapper.Map(request, item);
                await _db.SaveChangesAsync();
                response.Success = true;
                response.Message = "Updated succesfully";
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }


    }
}
