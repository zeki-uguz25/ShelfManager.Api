using Core.Exception.Exceptions;
using Core.Exception.Resources;
using Core.Extensions;
using Core.Persistence.EntityFrameworkCore.UnitOfWork;
using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Application.Abstractions.Services;
using ShelfManager.Application.Constants;
using ShelfManager.Domain.Common;
using ShelfManager.Domain.Entities;

namespace ShelfManager.Application.Handlers.Fines.Commands
{
    public class PayFineCommandResponse
    {
        public string Message { get; set; } = null!;
    }

    public class PayFineCommandRequest : IRequest<PayFineCommandResponse>
    {
        public Guid Id { get; set; }
    }

    public class PayFineCommandHandler : IRequestHandler<PayFineCommandRequest, PayFineCommandResponse>
    {
        private readonly IFineRepository _fineRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public PayFineCommandHandler(IFineRepository fineRepository, INotificationRepository notificationRepository, IAuthService authService, IUnitOfWork unitOfWork)
        {
            _fineRepository = fineRepository;
            _notificationRepository = notificationRepository;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        public async Task<PayFineCommandResponse> Handle(PayFineCommandRequest request, CancellationToken cancellationToken)
        {
            var userId = _authService.GetCurrentUserId();
            var fine = await _fineRepository.GetByIdAsync(request.Id);
            (fine == null).IfTrueThrow(() => new NotFoundException(ExceptionsResources.FineNotFound));
            (fine!.UserId != userId).IfTrueThrow(() => new BusinessException(ExceptionsResources.FineNotOwned));
            fine.IsPaid.IfTrueThrow(() => new BusinessException(ExceptionsResources.FineAlreadyPaid));

            fine.IsPaid = true;
            await _fineRepository.UpdateAsync(fine);

            var notification = new Notification
            {
                UserId = userId,
                Message = NotificationMessages.FinePaid,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            await _notificationRepository.AddAsync(notification);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new PayFineCommandResponse {};
        }
    }
}
