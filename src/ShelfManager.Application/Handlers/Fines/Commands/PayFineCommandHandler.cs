using Core.Exception.Exceptions;
using Core.Exception.Resources;
using Core.Persistence.EntityFrameworkCore.UnitOfWork;
using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Application.Abstractions.Services;

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
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public PayFineCommandHandler(IFineRepository fineRepository, IAuthService authService, IUnitOfWork unitOfWork)
        {
            _fineRepository = fineRepository;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        public async Task<PayFineCommandResponse> Handle(PayFineCommandRequest request, CancellationToken cancellationToken)
        {
            var userId = _authService.GetCurrentUserId();
            var fine = await _fineRepository.GetByIdAsync(request.Id);
            if (fine == null) throw new NotFoundException(ExceptionsResources.FineNotFound);
            if (fine.UserId != userId) throw new BusinessException(ExceptionsResources.FineNotOwned);
            if (fine.IsPaid) throw new BusinessException(ExceptionsResources.FineAlreadyPaid);

            fine.IsPaid = true;
            await _fineRepository.UpdateAsync(fine);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new PayFineCommandResponse { Message = "Ceza başarıyla ödendi." };
        }
    }
}
