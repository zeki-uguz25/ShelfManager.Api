using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Application.Abstractions.Services;
using ShelfManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    {//Kullanıcının kitap iade işlemi
        //Userbook da ısReturned true yapılacak
        //Kitabın stok sayısı 1 arttırılacak
        //ceza kontrolü yapılmalı eğer cezalı ise response de bildirilmeli

        private readonly IUserBookRepository _userBookRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IFineRepository _fineRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public ReturnBookCommandHandler(
            IUserBookRepository userBookRepository,
            IBookRepository bookRepository,
            IFineRepository fineRepository,
            IUserRepository userRepository,
            IAuthService authService
            )
        {
            _userBookRepository = userBookRepository;
            _bookRepository = bookRepository;
            _fineRepository = fineRepository;
            _userRepository = userRepository;
            _authService = authService;
        }

        public async Task<ReturnBookCommandResponse> Handle(ReturnBookCommandRequest request, CancellationToken cancellationToken)
        {
            var userId= _authService.GetCurrentUserId();
            var userBook = await _userBookRepository.GetByIdAsync( request.Id );
            if( userBook == null )
            {
                throw new Exception("Kullanıcı ve kitap kaydı bulunamadı.");
            }
            if(userBook.UserId!=userId)
            {
                throw new Exception("Bu kitap size ait değil");
            }

            var book = await _bookRepository.GetByIdAsync( userBook.BookId );
            var user= await _userRepository.GetByIdAsync(userId );
            if (user == null) throw new Exception("Kullanıcı bulunamadı.");
            if (book == null) throw new Exception("Kitap bulunamadı.");

            var fines = await _fineRepository.GetUnpaidFinesByUserIdAsync(userId);
            string? fineMessage=null;
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
            {
                fineMessage = "Cezanız bulunmaktadır.";
            }
            var requestRating=request.Rating;
            if (requestRating < 0 || requestRating > 5)
            {
                throw new Exception("Kitap puanı 0-5 arası olmalıdır.");
            }



            userBook.IsReturned = true;
            book.StockCount++;
            userBook.Rating = request.Rating;
            userBook.Comment= request.Comment;
            await _bookRepository.UpdateAsync( book );
            await _userBookRepository.UpdateAsync(userBook );

            return new ReturnBookCommandResponse
            {
                Message = fineMessage
            };
        }


    }
}
