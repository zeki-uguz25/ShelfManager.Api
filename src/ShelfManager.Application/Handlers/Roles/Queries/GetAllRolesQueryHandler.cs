using MediatR;
using ShelfManager.Application.Abstractions.Repositories;

namespace ShelfManager.Application.Handlers.Roles.Queries
{
    public class GetAllRolesQueryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class GetAllRolesQueryRequest : IRequest<IEnumerable<GetAllRolesQueryResponse>>
    {
    }

    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQueryRequest, IEnumerable<GetAllRolesQueryResponse>>
    {
        private readonly IRoleRepository _roleRepository;

        public GetAllRolesQueryHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<GetAllRolesQueryResponse>> Handle(GetAllRolesQueryRequest request, CancellationToken cancellationToken)
        {
            var roles = await _roleRepository.GetAllAsync();

            return roles.Select(x => new GetAllRolesQueryResponse
            {
                Id = x.Id,
                Name = x.Name
            });
        }
    }
}
