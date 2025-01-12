
using AutoMapper;
using ElectionMaterialManager.AppUserContext;
using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using ElectionMaterialManager.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.CQRS.Commands.UserCommands.AddAddress
{
    public class AddAddressCommandHandler : IRequestHandler<AddAddressCommand, Response>
    {
        private readonly ElectionMaterialManagerContext _db;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public AddAddressCommandHandler(ElectionMaterialManagerContext db, IMapper mapper, IUserContext userContext)
        {
            _db = db;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<Response> Handle(AddAddressCommand request, CancellationToken cancellationToken)
        {
            var response = new Response() { Success = false, StatusCode = 400 };
            try
            {
                var currentUser = await _userContext.GetCurrentUser();
                bool isEditable = currentUser != null;
                if (!isEditable)
                {
                    response.Message = "User is not authorized to access";
                    response.StatusCode = 401;
                    return response;
                }

                var address = new Address()
                {
                    City = request.City,
                    Country = request.Country,
                    Street = request.Street,
                    PostalCode = request.PostalCode,
                    UserId = currentUser.Id
                };


                _db.Add(address);
                await _db.SaveChangesAsync();
                response.Success = true;
                response.StatusCode = 201;
               // response.Message = $"/api/v1/";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
