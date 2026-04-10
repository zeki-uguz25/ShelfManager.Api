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

        public UnbanUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UnbanUserCommandResponse> Handle(UnbanUserCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");

            if (!user.IsBanned)
                throw new Exception("Kullanıcı zaten banlı değil.");

            user.IsBanned = false;
            await _userRepository.UpdateAsync(user);

            return new UnbanUserCommandResponse { Message = "Kullanıcının banı kaldırıldı." };
        }
    }
}
