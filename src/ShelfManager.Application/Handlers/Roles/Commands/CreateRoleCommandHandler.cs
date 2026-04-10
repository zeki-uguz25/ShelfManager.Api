using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
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

    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommandRequest, CreateRoleCommandResponse>
    {
        private readonly IRoleRepository _roleRepository;

        public CreateRoleCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<CreateRoleCommandResponse> Handle(CreateRoleCommandRequest request, CancellationToken cancellationToken)
        {
            var existing = await _roleRepository.GetByNameAsync(request.Name);
            if (existing != null)
                throw new Exception("Bu isimde bir rol zaten mevcut.");

            var role = new Role
            {
                Id = Guid.NewGuid(),
                Name = request.Name
            };

            await _roleRepository.AddAsync(role);

            return new CreateRoleCommandResponse
            {
                Id = role.Id,
                Name = role.Name
            };
        }
    }
}
