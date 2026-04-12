using Core.Exception.Exceptions;
using Core.Exception.Resources;
using Core.Persistence.EntityFrameworkCore.UnitOfWork;
using MediatR;
using ShelfManager.Application.Abstractions.Repositories;

namespace ShelfManager.Application.Handlers.Users.Commands
{
    public class UnbanUserCommandResponse
    {
        public string Message { get; set; } = null!;
    }

    public class UnbanUserCommandRequest : IRequest<UnbanUserCommandResponse>
    {
        public Guid Id { get; set; }
    }

    public class UnbanUserCommandHandler : IRequestHandler<UnbanUserCommandRequest, UnbanUserCommandResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UnbanUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UnbanUserCommandResponse> Handle(UnbanUserCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
                throw new NotFoundException(ExceptionsResources.UserNotFound);

            if (!user.IsBanned)
                throw new BusinessException(ExceptionsResources.UserNotBanned);

            user.IsBanned = false;
            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UnbanUserCommandResponse { Message = "Kullanıcının banı kaldırıldı." };
        }
    }
}
