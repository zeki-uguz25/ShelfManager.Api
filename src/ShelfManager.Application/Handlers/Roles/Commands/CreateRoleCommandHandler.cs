using Core.Exception.Exceptions;
using Core.Exception.Resources;
using Core.Extensions;
using Core.Persistence.EntityFrameworkCore.UnitOfWork;
using FluentValidation;
using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Application.Handlers.Roles.Resources;
using ShelfManager.Domain.Entities;

namespace ShelfManager.Application.Handlers.Roles.Commands
{
    public class CreateRoleCommandResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class CreateRoleCommandRequest : IRequest<CreateRoleCommandResponse>
    {
        public string Name { get; set; } = null!;
    }

    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommandRequest>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(_ => ValidationMessages.Name_Required)
                .MaximumLength(100).WithMessage(_ => ValidationMessages.Name_MaxLength);
        }
    }

    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommandRequest, CreateRoleCommandResponse>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRoleCommandHandler(IRoleRepository roleRepository, IUnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateRoleCommandResponse> Handle(CreateRoleCommandRequest request, CancellationToken cancellationToken)
        {
            var existing = await _roleRepository.GetByNameAsync(request.Name);
            (existing != null).IfTrueThrow(() => new BusinessException(ExceptionsResources.RoleAlreadyExists));

            var role = new Role
            {
                Id = Guid.NewGuid(),
                Name = request.Name
            };

            await _roleRepository.AddAsync(role);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateRoleCommandResponse
            {
                Id = role.Id,
                Name = role.Name
            };
        }
    }
}
