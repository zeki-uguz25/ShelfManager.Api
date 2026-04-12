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

        public GetUserFinesQueryHandler(IFineRepository fineRepository, IUserRepository userRepository, IAuthService authService)
        {
            _fineRepository = fineRepository;
            _userRepository = userRepository;
            _authService = authService;
        }

        public async Task<IEnumerable<GetUserFinesQueryResponse>> Handle(GetUserFinesQueryRequest request, CancellationToken cancellationToken)
        {
            var userId = _authService.GetCurrentUserId();
            var fines = await _fineRepository.GetAllByUserIdAsync(userId);

            return fines.Select(x => new GetUserFinesQueryResponse
            {
                Id = x.Id,
                Amount = x.Amount,
                IsPaid = x.IsPaid,
                CreatedAt = x.CreatedAt
            });
        }
    }
}
