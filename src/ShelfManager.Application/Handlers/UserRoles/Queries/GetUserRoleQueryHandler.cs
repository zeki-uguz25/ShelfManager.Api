using Core.Exception.Exceptions;
using Core.Exception.Resources;
using MediatR;
using ShelfManager.Application.Abstractions.Repositories;

namespace ShelfManager.Application.Handlers.UserRoles.Queries
{
    public class GetUserRoleQueryResponse
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; } = null!;
    }

    public class GetUserRoleQueryRequest : IRequest<GetUserRoleQueryResponse>
    {
        public Guid UserId { get; set; }
    }

    public class GetUserRoleQueryHandler : IRequestHandler<GetUserRoleQueryRequest, GetUserRoleQueryResponse>
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserRepository _userRepository;

        public GetUserRoleQueryHandler(IUserRoleRepository userRoleRepository, IUserRepository userRepository)
        {
            _userRoleRepository = userRoleRepository;
            _userRepository = userRepository;
        }

        public async Task<GetUserRoleQueryResponse> Handle(GetUserRoleQueryRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                throw new NotFoundException(ExceptionsResources.UserNotFound);

            var userRoles = await _userRoleRepository.GetByUserIdAsync(request.UserId);
            var userRole = userRoles.FirstOrDefault();
            if (userRole == null)
                throw new NotFoundException(ExceptionsResources.UserRoleNotFound);

            return new GetUserRoleQueryResponse
            {
                RoleId = userRole.RoleId,
                RoleName = userRole.Role.Name
            };
        }
    }
}
