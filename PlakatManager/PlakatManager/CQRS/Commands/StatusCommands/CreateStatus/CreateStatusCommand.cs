using ElectionMaterialManager.CQRS.Responses;
using ElectionMaterialManager.Dtos;
using MediatR;

namespace ElectionMaterialManager.CQRS.Commands.StatusCommands.CreateStatus
{
    public class CreateStatusCommand: IRequest<GenericResponse<StatusDto>>
    {
        public string Name { get; set; }
    }
}
