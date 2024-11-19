using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.EditElectionItem
{
    public class EditElectionItemCommandHandler: IRequestHandler<EditElectionItemCommand>
    {

        private readonly ElectionMaterialManagerContext _db;
        private readonly IMapper _mapper;

        public EditElectionItemCommandHandler(ElectionMaterialManagerContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task Handle(EditElectionItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _db.ElectionItems.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (item == null) return ;

            var xd = _mapper.Map(request,item);
            await _db.SaveChangesAsync();

            return ;
        }


    }
}
