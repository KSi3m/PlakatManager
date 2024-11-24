﻿using AutoMapper;
using ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateBillboard;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using ElectionMaterialManager.Utilities;
using MediatR;

namespace ElectionMaterialManager.CQRS.Commands.ElectionItemsCommands.CreateElectionItem
{
    public class CreateElectionItemCommandHandler : IRequestHandler<CreateElectionItemCommand, GenericResponse<ElectionItemDto>>
    {

        private readonly ElectionMaterialManagerContext _db;
        private readonly ElectionItemFactoryRegistry _factoryRegistry;
        private readonly IMapper _mapper;

        public CreateElectionItemCommandHandler(ElectionMaterialManagerContext db, 
            ElectionItemFactoryRegistry factoryRegistry, IMapper mapper)
        {
            _db = db;
            _factoryRegistry = factoryRegistry;
            _mapper = mapper;
        }

        public async Task<GenericResponse<ElectionItemDto>> Handle(CreateElectionItemCommand request, CancellationToken cancellationToken)
        {
            var response = new GenericResponse<ElectionItemDto>() { Success = false };
            try
            {
                var type = request.Type;
                var electionItem = _factoryRegistry.CreateElectionItem(type, request);

                _db.Add(electionItem);
                await _db.SaveChangesAsync();

                response.Success = true;
                response.Data = _mapper.Map<ElectionItemDto>(electionItem);
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
