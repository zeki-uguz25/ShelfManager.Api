using Core.Exception.Exceptions;
using Core.Exception.Resources;
using Core.Persistence.EntityFrameworkCore.UnitOfWork;
using MediatR;
using ShelfManager.Application.Abstractions.Repositories;

namespace ShelfManager.Application.Handlers.Users.Commands
{
    public class BanUserCommandResponse
    {
        public string Message { get; set; } = null!;
    }

    public class BanUserCommandRequest : IRequest<BanUserCommandResponse>
    {
        public Guid Id { get; set; }
    }

    public class BanUserCommandHandler : IRequestHandler<BanUserCommandRequest, BanUserCommandResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BanUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BanUserCommandResponse> Handle(BanUserCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
                throw new NotFoundException(ExceptionsResources.UserNotFound);

            if (user.IsBanned)
                throw new BusinessException(ExceptionsResources.UserAlreadyBanned);

            user.IsBanned = true;
            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new BanUserCommandResponse { Message = "Kullanıcı banlandı." };
        }
    }
}
