using Core.Exception.Exceptions;
using Core.Exception.Resources;
using Core.Extensions;
using Core.Persistence.EntityFrameworkCore.UnitOfWork;
using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Application.Abstractions.Services;
using ShelfManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShelfManager.Application.Handlers.Books.Commands
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
        private readonly IBookCacheService _bookCacheService;

        public BorrowBookCommandHandler(
            IUserBookRepository userBookRepository,
            IBookRepository bookRepository,
            IUserRepository userRepository,
            IAuthService authService,
            IUnitOfWork unitOfWork,
            IBookCacheService bookCacheService)
        {
            _userBookRepository = userBookRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _authService = authService;
            _unitOfWork = unitOfWork;
            _bookCacheService = bookCacheService;
        }

        public async Task<BorrowBookCommandResponse> Handle(BorrowBookCommandRequest request, CancellationToken cancellationToken)
        {
            var userId = _authService.GetCurrentUserId();
            var user = await _userRepository.GetByIdAsync(userId);//user burda atanýyor ama stok kontrolünden sonra kullanýlýyor
            var book = await _bookRepository.GetByIdAsync(request.BookId);

            (book == null).IfTrueThrow(() => new NotFoundException(ExceptionsResources.BookNotFound));

            (book!.StockCount <= 0).IfTrueThrow(() => new BusinessException(ExceptionsResources.BookOutOfStock));
            user!.IsBanned.IfTrueThrow(() => new BusinessException(ExceptionsResources.UserBanned));

            var userBooks = await _userBookRepository.GetBorrowedBookCountByUserIdAsync(userId);
            (userBooks.Count() >= 3).IfTrueThrow(() => new BusinessException(ExceptionsResources.MaxBorrowLimitReached));

            var isConflict = userBooks.Any(x => x.BookId == book.Id);
            isConflict.IfTrueThrow(() => new BusinessException(ExceptionsResources.BookAlreadyBorrowed));

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
            await _bookCacheService.InvalidateAsync(cancellationToken);
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
