using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShelfManager.Application.Handlers.Books.Queries
{
    public class GetUserBooksQueryResponse
    {
        public Guid BookId { get; set; }
        public string BookName { get; set; } = null!;
        public DateTime BorrowDate { get; set; }
        public int? Rating { get; set; } //İade ederken verilen puan
        public bool IsReturned { get; set; }
        public string? Comment { get; set; }
    }
    public class GetUserBooksQueryRequest : IRequest<IEnumerable<GetUserBooksQueryResponse>>
    {
    }

    public class GetUserBooksQueryHandler : IRequestHandler<GetUserBooksQueryRequest, IEnumerable<GetUserBooksQueryResponse>>
    {
        private readonly IUserBookRepository _userBookRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IAuthService _authService;
        public GetUserBooksQueryHandler(IUserBookRepository userBookRepository,
            IUserRepository userRepository,
            IBookRepository bookRepository,
            IAuthService authService
            )
        {
            _bookRepository = bookRepository;
            _userBookRepository = userBookRepository;
            _userRepository = userRepository;
            _authService = authService;
        }

        public async Task<IEnumerable<GetUserBooksQueryResponse>> Handle(GetUserBooksQueryRequest request, CancellationToken cancellationToken)
        {
            var userId = _authService.GetCurrentUserId();

            var userBook = await _userBookRepository.GetAllBookByUserIdAsync(userId);

            return userBook.Select(x => new GetUserBooksQueryResponse
            {
                BookId = x.BookId,
                BookName = x.Book.Name,
                BorrowDate = x.BorrowDate,
                Rating = x.Rating,
                IsReturned = x.IsReturned,
                Comment = x.Comment

            });
        }
    }
}
