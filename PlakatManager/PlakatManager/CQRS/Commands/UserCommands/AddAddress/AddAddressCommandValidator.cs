using Azure.Core;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Metrics;
using System.IO;

namespace ElectionMaterialManager.CQRS.Commands.UserCommands.AddAddress
{
    public class AddAddressCommandValidator: AbstractValidator<AddAddressCommand>
    {

        public AddAddressCommandValidator()
        {

            RuleFor(x => x.PostalCode).MaximumLength(20);
            RuleFor(x => x.Country).NotEmpty();
 
        }
    }
}
