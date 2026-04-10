using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Application.Abstractions.Services;

namespace ShelfManager.Application.Handlers.Notifications.Commands
{
    public class MarkAsReadCommandResponse
    {
        public string Message { get; set; } = null!;
    }

    public class MarkAsReadCommandRequest : IRequest<MarkAsReadCommandResponse>
    {
        public Guid Id { get; set; }
    }

    public class MarkAsReadCommandHandler : IRequestHandler<MarkAsReadCommandRequest, MarkAsReadCommandResponse>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IAuthService _authService;

        public MarkAsReadCommandHandler(INotificationRepository notificationRepository, IAuthService authService)
        {
            _notificationRepository = notificationRepository;
            _authService = authService;
        }

        public async Task<MarkAsReadCommandResponse> Handle(MarkAsReadCommandRequest request, CancellationToken cancellationToken)
        {
            var userId= _authService.GetCurrentUserId();
            var notification = await _notificationRepository.GetByIdAsync(request.Id);
            if (notification == null)
                throw new Exception("Bildirim bulunamadı.");
            if (notification.UserId != userId)
            {
                throw new Exception("Bu Bildirim size ait değil.");
            }

            notification.IsRead = true;
            await _notificationRepository.UpdateAsync(notification);

            return new MarkAsReadCommandResponse { Message = "Bildirim okundu olarak işaretlendi." };
        }
    }
}
