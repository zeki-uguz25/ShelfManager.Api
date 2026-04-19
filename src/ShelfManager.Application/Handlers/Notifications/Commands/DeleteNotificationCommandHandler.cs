using Core.Exception.Exceptions;
using Core.Exception.Resources;
using Core.Extensions;
using Core.Persistence.EntityFrameworkCore.UnitOfWork;
using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Application.Abstractions.Services;

namespace ShelfManager.Application.Handlers.Notifications.Commands;

public class DeleteNotificationCommandResponse { }

public class DeleteNotificationCommandRequest : IRequest<DeleteNotificationCommandResponse>
{
    public Guid Id { get; set; }
}

public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommandRequest, DeleteNotificationCommandResponse>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteNotificationCommandHandler(INotificationRepository notificationRepository, IAuthService authService, IUnitOfWork unitOfWork)
    {
        _notificationRepository = notificationRepository;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteNotificationCommandResponse> Handle(DeleteNotificationCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _authService.GetCurrentUserId();

        var notification = await _notificationRepository.GetByIdAsync(request.Id);
        (notification == null).IfTrueThrow(() => new NotFoundException(ExceptionsResources.NotificationNotFound));
        (notification!.UserId != userId).IfTrueThrow(() => new BusinessException(ExceptionsResources.NotificationNotOwned));

        await _notificationRepository.SoftDeleteAsync(notification);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new DeleteNotificationCommandResponse();
    }
}
