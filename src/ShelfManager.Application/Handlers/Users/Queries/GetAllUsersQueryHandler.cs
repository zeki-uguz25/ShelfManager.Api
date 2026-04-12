using Core.Persistence.EntityFrameworkCore.Pagination;
using MediatR;
using ShelfManager.Application.Abstractions.Repositories;

namespace ShelfManager.Application.Handlers.Users.Queries
{
    public class GetAllUsersQueryResponse
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public bool IsBanned { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class GetAllUsersQueryRequest : PagedRequest, IRequest<PagedList<GetAllUsersQueryResponse>>
    {
    }

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQueryRequest, PagedList<GetAllUsersQueryResponse>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<PagedList<GetAllUsersQueryResponse>> Handle(GetAllUsersQueryRequest request, CancellationToken cancellationToken)
        {
            var paged = await _userRepository.GetPagedAsync(request.PageNumber, request.PageSize);

            return new PagedList<GetAllUsersQueryResponse>
            {
                Items = paged.Items.Select(x => new GetAllUsersQueryResponse
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    IsBanned = x.IsBanned,
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt
                }).ToList(),
                TotalCount = paged.TotalCount,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize
            };
        }
    }
}
