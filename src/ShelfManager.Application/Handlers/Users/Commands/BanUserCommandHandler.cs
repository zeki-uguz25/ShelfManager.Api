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

        public BanUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<BanUserCommandResponse> Handle(BanUserCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");

            if (user.IsBanned)
                throw new Exception("Kullanıcı zaten banlı.");

            user.IsBanned = true;
            await _userRepository.UpdateAsync(user);

            return new BanUserCommandResponse { Message = "Kullanıcı banlandı." };
        }
    }
}
