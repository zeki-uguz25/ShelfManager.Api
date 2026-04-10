using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
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

    public class AssignRoleCommandHandler : IRequestHandler<AssignRoleCommandRequest, AssignRoleCommandResponse>
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public AssignRoleCommandHandler(
            IUserRoleRepository userRoleRepository,
            IUserRepository userRepository,
            IRoleRepository roleRepository)
        {
            _userRoleRepository = userRoleRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<AssignRoleCommandResponse> Handle(AssignRoleCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");

            var role = await _roleRepository.GetByIdAsync(request.RoleId);
            if (role == null)
                throw new Exception("Rol bulunamadı.");

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

            return new AssignRoleCommandResponse { Message = $"Kullanıcıya '{role.Name}' rolü atandı." };
        }
    }
}
