using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Application.Abstractions.Services;

namespace ShelfManager.Application.Handlers.Notifications.Queries
{
    public class GetUserNotificationsQueryResponse
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = null!;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class GetUserNotificationsQueryRequest : IRequest<IEnumerable<GetUserNotificationsQueryResponse>>
    {
        
    }

    public class GetUserNotificationsQueryHandler : IRequestHandler<GetUserNotificationsQueryRequest, IEnumerable<GetUserNotificationsQueryResponse>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public GetUserNotificationsQueryHandler(INotificationRepository notificationRepository, IUserRepository userRepository, IAuthService authService)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _authService = authService;
        }

        public async Task<IEnumerable<GetUserNotificationsQueryResponse>> Handle(GetUserNotificationsQueryRequest request, CancellationToken cancellationToken)
        {

            var userId = _authService.GetCurrentUserId();

            var notifications = await _notificationRepository.GetAllByUserIdAsync(userId);

            return notifications.Select(x => new GetUserNotificationsQueryResponse
            {
                Id = x.Id,
                Message = x.Message,
                IsRead = x.IsRead,
                CreatedAt = x.CreatedAt
            });
        }
    }
}
