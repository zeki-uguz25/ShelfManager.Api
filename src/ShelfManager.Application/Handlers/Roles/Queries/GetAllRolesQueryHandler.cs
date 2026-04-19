using AutoMapper;
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
        private readonly IMapper _mapper;

        public GetAllRolesQueryHandler(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllRolesQueryResponse>> Handle(GetAllRolesQueryRequest request, CancellationToken cancellationToken)
        {
            var roles = await _roleRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<GetAllRolesQueryResponse>>(roles);
        }
    }
}
