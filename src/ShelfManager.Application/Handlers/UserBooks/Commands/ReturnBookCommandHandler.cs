using Core.Exception.Exceptions;
using Core.Exception.Resources;
using Core.Persistence.EntityFrameworkCore.UnitOfWork;
using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Application.Abstractions.Services;
using ShelfManager.Domain.Entities;

namespace ShelfManager.Application.Handlers.UserBooks.Commands
{
    public class ReturnBookCommandResponse
    {
        public string? Message { get; set; }
    }

    public class ReturnBookCommandRequest : IRequest<ReturnBookCommandResponse>
    {
        public Guid Id { get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }
    }

    public class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommandRequest, ReturnBookCommandResponse>
    {
        private readonly IUserBookRepository _userBookRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IFineRepository _fineRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public ReturnBookCommandHandler(
            IUserBookRepository userBookRepository,
            IBookRepository bookRepository,
            IFineRepository fineRepository,
            IUserRepository userRepository,
            IAuthService authService,
            IUnitOfWork unitOfWork)
        {
            _userBookRepository = userBookRepository;
            _bookRepository = bookRepository;
            _fineRepository = fineRepository;
            _userRepository = userRepository;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ReturnBookCommandResponse> Handle(ReturnBookCommandRequest request, CancellationToken cancellationToken)
        {
            var userId = _authService.GetCurrentUserId();
            var userBook = await _userBookRepository.GetByIdAsync(request.Id);
            if (userBook == null) throw new NotFoundException(ExceptionsResources.UserBookNotFound);
            if (userBook.UserId != userId) throw new BusinessException(ExceptionsResources.UserBookNotOwned);

            var book = await _bookRepository.GetByIdAsync(userBook.BookId);
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new NotFoundException(ExceptionsResources.UserNotFound);
            if (book == null) throw new NotFoundException(ExceptionsResources.BookNotFound);

            var requestRating = request.Rating;
            if (requestRating < 0 || requestRating > 5)
                throw new BusinessException(ExceptionsResources.InvalidRating);

            var fines = await _fineRepository.GetUnpaidFinesByUserIdAsync(userId);
            string? fineMessage = null;
            bool isLate = userBook.ReturnDeadline < DateTime.UtcNow;
            if (isLate)
            {
                var fine = new Fine
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    UserBookId = userBook.Id,
                    Amount = 10,
                    IsPaid = false,
                    CreatedAt = DateTime.UtcNow
                };
                await _fineRepository.AddAsync(fine);
            }
            if (fines.Any())
                fineMessage = "Cezanız bulunmaktadır.";

            userBook.IsReturned = true;
            book.StockCount++;
            userBook.Rating = request.Rating;
            userBook.Comment = request.Comment;

            await _bookRepository.UpdateAsync(book);
            await _userBookRepository.UpdateAsync(userBook);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ReturnBookCommandResponse { Message = fineMessage };
        }
    }
}
