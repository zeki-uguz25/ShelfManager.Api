using AutoMapper;
using Core.Exception.Exceptions;
using Core.Exception.Resources;
using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Application.Abstractions.Services;

namespace ShelfManager.Application.Handlers.Fines.Queries
{
    public class GetUserFinesQueryResponse
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class GetUserFinesQueryRequest : IRequest<IEnumerable<GetUserFinesQueryResponse>>
    {
    }

    public class GetUserFinesQueryHandler : IRequestHandler<GetUserFinesQueryRequest, IEnumerable<GetUserFinesQueryResponse>>
    {
        private readonly IFineRepository _fineRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public GetUserFinesQueryHandler(IFineRepository fineRepository, IUserRepository userRepository, IAuthService authService, IMapper mapper)
        {
            _fineRepository = fineRepository;
            _userRepository = userRepository;
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetUserFinesQueryResponse>> Handle(GetUserFinesQueryRequest request, CancellationToken cancellationToken)
        {
            var userId = _authService.GetCurrentUserId();
            var fines = await _fineRepository.GetAllByUserIdAsync(userId);

            return _mapper.Map<IEnumerable<GetUserFinesQueryResponse>>(fines);
        }
    }
}
