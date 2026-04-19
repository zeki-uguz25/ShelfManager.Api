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
            (userBook == null).IfTrueThrow(() => new NotFoundException(ExceptionsResources.UserBookNotFound));
            (userBook!.UserId != userId).IfTrueThrow(() => new BusinessException(ExceptionsResources.UserBookNotOwned));

            var book = await _bookRepository.GetByIdAsync(userBook.BookId);
            var user = await _userRepository.GetByIdAsync(userId);
            (user == null).IfTrueThrow(() => new NotFoundException(ExceptionsResources.UserNotFound));
            (book == null).IfTrueThrow(() => new NotFoundException(ExceptionsResources.BookNotFound));

            var requestRating = request.Rating;
            (requestRating < 0 || requestRating > 5).IfTrueThrow(() => new BusinessException(ExceptionsResources.InvalidRating));

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
                    CreatedAt = DateTime.UtcNow,
                };
                await _fineRepository.AddAsync(fine);
            }
            if (fines.Any())
                fineMessage = "Cezanız bulunmaktadır.";

            userBook.IsReturned = true;
            book!.StockCount++;
            userBook.Rating = request.Rating;
            userBook.Comment = request.Comment;

            await _bookRepository.UpdateAsync(book);
            await _userBookRepository.UpdateAsync(userBook);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ReturnBookCommandResponse { Message = fineMessage };
        }
    }
}
