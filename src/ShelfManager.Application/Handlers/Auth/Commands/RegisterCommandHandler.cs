using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Application.Abstractions.Services;
using ShelfManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShelfManager.Application.Handlers.Auth.Commands
{

    public class RegisterCommandResponse
    {
        public string Token { get; set; } = null!;
    }

    public class RegisterCommandRequest : IRequest<RegisterCommandResponse>
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

    }
    public class RegisterCommandHandler : IRequestHandler<RegisterCommandRequest, RegisterCommandResponse> 
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IHashingService _hashingService;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public RegisterCommandHandler (
            IUserRepository userRepository,
            ITokenService tokenService,
            IHashingService hashingService,
            IRoleRepository roleRepository,
            IUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _hashingService = hashingService;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<RegisterCommandResponse> Handle(RegisterCommandRequest request, CancellationToken cancellationToken)
        {
            var conflict = await _userRepository.GetByEmailAsync(request.Email);
            if (conflict != null)
            {
                throw new Exception("Bu email zaten sisteme kayıtlı");
            }
            var passwordHash = _hashingService.HashPassword(request.Password, out string salt);
            var role = await _roleRepository.GetByNameAsync("Member");
            if (role == null) throw new Exception("Member rolü bulunamadı.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = request.FullName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = passwordHash,
                PasswordSalt = salt,
                IsBanned = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Address = request.Address,
            };
            await _userRepository.AddAsync(user);
            var userRole = new UserRole
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                RoleId = role.Id
            };
            await _userRoleRepository.AddAsync(userRole);

            

            var token = _tokenService.GenerateToken(user, new List<string> { role.Name });

            return new RegisterCommandResponse { Token = token };




        }


    }
}
