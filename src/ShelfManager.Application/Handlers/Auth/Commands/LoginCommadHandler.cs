using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShelfManager.Application.Handlers.Auth.Commands
{
    public class LoginCommandResponse
    {
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


        public LoginCommadHandler(
            IUserRepository userRepository,
            IHashingService hashingService, ITokenService tokenService,IUserRoleRepository userRoleRepository
            )
        {
            _hashingService= hashingService;
            _userRepository= userRepository;
            _tokenService= tokenService;
            _userRoleRepository= userRoleRepository;
        }

        public async Task<LoginCommandResponse> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email.ToLower());
            if (user == null)
            {
                throw new Exception("Sisteme kayıtlı değilsiniz");
            }

            var isPasswordMatch = _hashingService.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt);

            if( !isPasswordMatch )
            {
                throw new Exception("Girdiğiniz bilgiler hatalıdır.");
            }

            var userRoles = await _userRoleRepository.GetByUserIdAsync(user.Id);
            var roles= userRoles.Select(r => r.Role.Name).ToList();

            var Token = _tokenService.GenerateToken(user, roles);

            return new LoginCommandResponse
            {
                Token = Token,
            };



        }

    }
}
