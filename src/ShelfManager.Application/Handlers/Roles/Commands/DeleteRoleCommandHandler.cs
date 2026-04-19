using Core.Exception.Exceptions;
using Core.Exception.Resources;
using Core.Extensions;
using Core.Persistence.EntityFrameworkCore.UnitOfWork;
using MediatR;
using ShelfManager.Application.Abstractions.Repositories;

namespace ShelfManager.Application.Handlers.Roles.Commands
{
    public class DeleteRoleCommandResponse
    {
        public string Message { get; set; } = null!;
    }

    public class DeleteRoleCommandRequest : IRequest<DeleteRoleCommandResponse>
    {
        public Guid Id { get; set; }
    }

    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommandRequest, DeleteRoleCommandResponse>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRoleCommandHandler(IRoleRepository roleRepository, IUnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteRoleCommandResponse> Handle(DeleteRoleCommandRequest request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetByIdAsync(request.Id);
            (role == null).IfTrueThrow(() => new NotFoundException(ExceptionsResources.RoleNotFound));

            await _roleRepository.DeleteAsync(role);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new DeleteRoleCommandResponse { Message = "Rol silindi." };
        }
    }
}
