using Core.Exception.Exceptions;
using Core.Exception.Resources;
using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Application.Abstractions.Services;

namespace ShelfManager.Application.Handlers.Auth.Commands
{
    public class LoginCommandResponse
    {
        public Guid UserId { get; set; }
        public string Token { get; set; } = null!;
    }

    public class LoginCommandRequest : IRequest<LoginCommandResponse>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class LoginCommadHandler : IRequestHandler<LoginCommandRequest,LoginCommandResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashingService _hashingService;
        private readonly ITokenService _tokenService;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;

        public LoginCommadHandler(
            IUserRepository userRepository,
            IHashingService hashingService,
            ITokenService tokenService,
            IUserRoleRepository userRoleRepository,
            IRoleRepository roleRepository)
        {
            _hashingService = hashingService;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
        }

        public async Task<LoginCommandResponse> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email.ToLower());
            if (user == null)
                throw new NotFoundException(ExceptionsResources.UserNotFound);

            var isPasswordMatch = _hashingService.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt);
            if (!isPasswordMatch)
                throw new BusinessException(ExceptionsResources.InvalidCredentials);

            var userRoles = await _userRoleRepository.GetByUserIdAsync(user.Id);
            var roles = userRoles.Select(r => r.Role.Name).ToList();

            var permissions = new List<string>();
            foreach (var userRole in userRoles)
            {
                var rolePermissions = await _roleRepository.GetPermissionsByRoleIdAsync(userRole.RoleId);
                permissions.AddRange(rolePermissions);
            }

            var token = _tokenService.GenerateToken(user, roles, permissions.Distinct().ToList());

            return new LoginCommandResponse
            {
                UserId = user.Id,
                Token = token
            };



        }

    }
}
