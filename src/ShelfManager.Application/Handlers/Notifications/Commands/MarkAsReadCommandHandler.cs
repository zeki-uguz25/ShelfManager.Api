using Core.Exception.Exceptions;
using Core.Exception.Resources;
using Core.Persistence.EntityFrameworkCore.UnitOfWork;
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
        private readonly IUnitOfWork _unitOfWork;

        public MarkAsReadCommandHandler(INotificationRepository notificationRepository, IAuthService authService, IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        public async Task<MarkAsReadCommandResponse> Handle(MarkAsReadCommandRequest request, CancellationToken cancellationToken)
        {
            var userId = _authService.GetCurrentUserId();
            var notification = await _notificationRepository.GetByIdAsync(request.Id);
            if (notification == null) throw new NotFoundException(ExceptionsResources.NotificationNotFound);
            if (notification.UserId != userId) throw new BusinessException(ExceptionsResources.NotificationNotOwned);

            notification.IsRead = true;
            await _notificationRepository.UpdateAsync(notification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new MarkAsReadCommandResponse { Message = "Bildirim okundu olarak işaretlendi." };
        }
    }
}
