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

        public PayFineCommandHandler(IFineRepository fineRepository,IAuthService authService)
        {
            _fineRepository = fineRepository;
            _authService = authService;
        }

        public async Task<PayFineCommandResponse> Handle(PayFineCommandRequest request, CancellationToken cancellationToken)
        {
            var userId= _authService.GetCurrentUserId();
            var fine = await _fineRepository.GetByIdAsync(request.Id);
            if (fine == null)
                throw new Exception("Ceza kaydı bulunamadı.");

            if (fine.UserId != userId)
                throw new Exception("Bu ceza size ait değil.");

            if (fine.IsPaid)
                throw new Exception("Bu ceza zaten ödenmiş.");

            fine.IsPaid = true;
            await _fineRepository.UpdateAsync(fine);

            return new PayFineCommandResponse { Message = "Ceza başarıyla ödendi." };
        }
    }
}
