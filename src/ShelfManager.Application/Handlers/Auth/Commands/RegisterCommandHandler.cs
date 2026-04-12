using Core.Exception.Exceptions;
using Core.Exception.Resources;
using FluentValidation;
using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Application.Abstractions.Services;
using ShelfManager.Domain.Entities;
using ShelfManager.Application.Handlers.Auth.Resources;
using RolesEnum = ShelfManager.Domain.Enums.Roles;
using Core.Persistence.EntityFrameworkCore.UnitOfWork;

namespace ShelfManager.Application.Handlers.Auth.Commands
{

    public class RegisterCommandResponse
    {
        public Guid UserId { get; set; }
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
    public class RegisterCommandValidator : AbstractValidator<RegisterCommandRequest>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage(_ => ValidationMessages.FullName_Required)
                .MaximumLength(200).WithMessage(_ => ValidationMessages.FullName_MaxLength);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(_ => ValidationMessages.Email_Required)
                .EmailAddress().WithMessage(_ => ValidationMessages.Email_Invalid);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(_ => ValidationMessages.Password_Required)
                .MinimumLength(6).WithMessage(_ => ValidationMessages.Password_MinLength);
        }
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommandRequest, RegisterCommandResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IHashingService _hashingService;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCommandHandler (
            IUserRepository userRepository,
            ITokenService tokenService,
            IHashingService hashingService,
            IRoleRepository roleRepository,
            IUserRoleRepository userRoleRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _hashingService = hashingService;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RegisterCommandResponse> Handle(RegisterCommandRequest request, CancellationToken cancellationToken)
        {
            var conflict = await _userRepository.GetByEmailAsync(request.Email);
            if (conflict != null)
                throw new BusinessException(ExceptionsResources.EmailAlreadyExists);

            var passwordHash = _hashingService.HashPassword(request.Password, out string salt);
            var role = await _roleRepository.GetByCodeAsync(nameof(RolesEnum.Member));
            if (role == null) throw new NotFoundException(ExceptionsResources.MemberRoleNotFound);

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
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            

            var permissions = await _roleRepository.GetPermissionsByRoleIdAsync(role.Id);
            var token = _tokenService.GenerateToken(user, new List<string> { role.Name }, permissions.ToList());

            return new RegisterCommandResponse { 
                UserId=user.Id,
                Token = token };




        }


    }
}
