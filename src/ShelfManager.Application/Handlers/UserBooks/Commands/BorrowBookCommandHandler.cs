using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Application.Abstractions.Services;
using ShelfManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

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
    public class BorrowBookCommandHandler : IRequestHandler<BorrowBookCommandRequest ,BorrowBookCommandResponse>
    {
        private readonly IUserBookRepository _userBookRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public BorrowBookCommandHandler(
            IUserBookRepository userBookRepository,
            IBookRepository bookRepository,
            IUserRepository userRepository,
            IAuthService authService
            )
        {
            _userBookRepository = userBookRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _authService = authService;
        }

        public async Task<BorrowBookCommandResponse> Handle(BorrowBookCommandRequest request, CancellationToken cancellationToken)
        {//user mevcut mu?, Kitap mevcut mu?, Elimizde bu kitaptan varmı?, User ın cezası varmı?, Bir User en fazla 3 kitap alabilir.
            var userId = _authService.GetCurrentUserId();
            var user = await _userRepository.GetByIdAsync( userId );
            var book = await _bookRepository.GetByIdAsync( request.BookId );
            

            if (user==null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }
            if(book==null)
            {
                throw new Exception("Kitap bulunamadı.");
            }

            int stockCount = book.StockCount;
            if(stockCount <= 0)
            {
                throw new Exception("Depoda mevcut değil.");
            }

            bool isBanned = user.IsBanned;
            if(isBanned)
            {
                throw new Exception("Sistem tarafından banlandınız. Cezalarını kontrol ediniz.");
            }
            var userBooks = await _userBookRepository.GetBorrowedBookCountByUserIdAsync(userId);
            var countUserBooks = userBooks.Count();
            if (countUserBooks >= 3)
            {
                throw new Exception("Aynı anda 3 kitap alamazsınız.");
            }

            var userBook = new UserBook
            {
                Id=Guid.NewGuid(),
                UserId=userId,
                BookId=book.Id,
                BorrowDate=DateTime.UtcNow,
                ReturnDeadline=DateTime.UtcNow.AddDays(30),
                IsReturned=false,
            };
            book.StockCount--;
            await _bookRepository.UpdateAsync(book);

            await _userBookRepository.AddAsync(userBook);

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
