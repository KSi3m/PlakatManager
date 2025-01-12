using ElectionMaterialManager.CQRS.Responses;
using MediatR;

namespace ElectionMaterialManager.CQRS.Commands.UserCommands.AddAddress
{
    public class AddAddressCommand: IRequest<Response>
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }

    }
}
