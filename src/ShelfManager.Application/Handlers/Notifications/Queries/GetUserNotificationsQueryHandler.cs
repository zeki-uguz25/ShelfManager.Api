using AutoMapper;
using Core.Exception.Exceptions;
using Core.Exception.Resources;
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
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public GetUserNotificationsQueryHandler(INotificationRepository notificationRepository, IAuthService authService, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetUserNotificationsQueryResponse>> Handle(GetUserNotificationsQueryRequest request, CancellationToken cancellationToken)
        {
            var userId = _authService.GetCurrentUserId();

            var notifications = await _notificationRepository.GetAllByUserIdAsync(userId);

            return _mapper.Map<IEnumerable<GetUserNotificationsQueryResponse>>(notifications);
        }
    }
}
