using Core.Exception.Exceptions;
using Core.Exception.Resources;
using Core.Persistence.EntityFrameworkCore.UnitOfWork;
using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Application.Abstractions.Services;
using ShelfManager.Domain.Entities;

namespace ShelfManager.Application.Handlers.UserBooks.Commands
{
    public class BorrowBookCommandResponse
    {
        public Guid Id { get; set; }
        public string BookName { get; set; } = null!;
        public string UserNName { get; set; } = null!;
        public DateTime ReturnDeadline { get; set; }
    }

    public class BorrowBookCommandRequest : IRequest<BorrowBookCommandResponse>
    {
        public Guid BookId { get; set; }
    }

    public class BorrowBookCommandHandler : IRequestHandler<BorrowBookCommandRequest, BorrowBookCommandResponse>
    {
        private readonly IUserBookRepository _userBookRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public BorrowBookCommandHandler(
            IUserBookRepository userBookRepository,
            IBookRepository bookRepository,
            IUserRepository userRepository,
            IAuthService authService,
            IUnitOfWork unitOfWork)
        {
            _userBookRepository = userBookRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        public async Task<BorrowBookCommandResponse> Handle(BorrowBookCommandRequest request, CancellationToken cancellationToken)
        {
            var userId = _authService.GetCurrentUserId();
            var user = await _userRepository.GetByIdAsync(userId);
            var book = await _bookRepository.GetByIdAsync(request.BookId);

            if (user == null) throw new NotFoundException(ExceptionsResources.UserNotFound);
            if (book == null) throw new NotFoundException(ExceptionsResources.BookNotFound);

            if (book.StockCount <= 0)
                throw new BusinessException(ExceptionsResources.BookOutOfStock);

            if (user.IsBanned)
                throw new BusinessException(ExceptionsResources.UserBanned);

            var userBooks = await _userBookRepository.GetBorrowedBookCountByUserIdAsync(userId);
            if (userBooks.Count() >= 3)
                throw new BusinessException(ExceptionsResources.MaxBorrowLimitReached);

            var userBook = new UserBook
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                BookId = book.Id,
                BorrowDate = DateTime.UtcNow,
                ReturnDeadline = DateTime.UtcNow.AddDays(30),
                IsReturned = false,
            };
            book.StockCount--;

            await _bookRepository.UpdateAsync(book);
            await _userBookRepository.AddAsync(userBook);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new BorrowBookCommandResponse
            {
                Id = book.Id,
                BookName = book.Name,
                UserNName = user.FullName,
                ReturnDeadline = DateTime.UtcNow.AddDays(30)
            };
        }
    }
}
