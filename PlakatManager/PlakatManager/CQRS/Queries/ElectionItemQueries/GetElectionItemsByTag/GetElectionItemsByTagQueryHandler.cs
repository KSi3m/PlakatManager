﻿using AutoMapper;
using Azure;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Queries.ElectionItemQueries.GetElectionItemsByTag
{
    public class GetElectionItemsByTagQueryHandler : IRequestHandler<GetElectionItemsByTagQuery, GenericResponseWithList<ElectionItemDto>>
    {

        private readonly ElectionMaterialManagerContext _db;
        private readonly IMapper _mapper;

        public GetElectionItemsByTagQueryHandler(ElectionMaterialManagerContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<GenericResponseWithList<ElectionItemDto>> Handle(GetElectionItemsByTagQuery query, CancellationToken cancellationToken)
        {

            var response = new GenericResponseWithList<ElectionItemDto>() { Data = [], Success = false };
            try
            {
                var electionItems = await _db.Tags.Include(x => x.ElectionItems)
                    .Where(x=>x.Value == query.TagName)
                    .SelectMany(x=>x.ElectionItems)
                    .ToListAsync();

                if (electionItems == null || !electionItems.Any())
                {
                    response.Message = "Election items with given tag not found.";
                    return response;
                }
                response.Message = "Election items with given tag found";
                response.Success = true;
              
                response.Data = _mapper.Map<IEnumerable<ElectionItemDto>>(electionItems);
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
            

        }
    }
}
