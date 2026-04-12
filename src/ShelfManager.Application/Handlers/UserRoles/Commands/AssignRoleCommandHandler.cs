using Core.Exception.Exceptions;
using Core.Exception.Resources;
using Core.Persistence.EntityFrameworkCore.UnitOfWork;
using FluentValidation;
using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Application.Handlers.UserRoles.Resources;
using ShelfManager.Domain.Entities;

namespace ShelfManager.Application.Handlers.UserRoles.Commands
{
    public class AssignRoleCommandResponse
    {
        public string Message { get; set; } = null!;
    }

    public class AssignRoleCommandRequest : IRequest<AssignRoleCommandResponse>
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }

    public class AssignRoleCommandValidator : AbstractValidator<AssignRoleCommandRequest>
    {
        public AssignRoleCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage(_ => ValidationMessages.UserId_Required);

            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage(_ => ValidationMessages.RoleId_Required);
        }
    }

    public class AssignRoleCommandHandler : IRequestHandler<AssignRoleCommandRequest, AssignRoleCommandResponse>
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssignRoleCommandHandler(
            IUserRoleRepository userRoleRepository,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork)
        {
            _userRoleRepository = userRoleRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AssignRoleCommandResponse> Handle(AssignRoleCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                throw new NotFoundException(ExceptionsResources.UserNotFound);

            var role = await _roleRepository.GetByIdAsync(request.RoleId);
            if (role == null)
                throw new NotFoundException(ExceptionsResources.RoleNotFound);

            var existingRoles = await _userRoleRepository.GetByUserIdAsync(request.UserId);
            var existingRole = existingRoles.FirstOrDefault();
            if (existingRole != null)
            {
                existingRole.RoleId = request.RoleId;
                await _userRoleRepository.UpdateAsync(existingRole);
            }
            else
            {
                var userRole = new UserRole
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    RoleId = request.RoleId
                };
                await _userRoleRepository.AddAsync(userRole);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new AssignRoleCommandResponse { Message = $"Kullanıcıya '{role.Name}' rolü atandı." };
        }
    }
}
