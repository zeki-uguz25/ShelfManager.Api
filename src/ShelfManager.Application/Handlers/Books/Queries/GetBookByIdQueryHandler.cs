using Core.Exception.Exceptions;
using Core.Exception.Resources;
using MediatR;
using ShelfManager.Application.Abstractions.Repositories;

namespace ShelfManager.Application.Handlers.Books.Queries
{
    public class GetBookByIdQuery
    {

        public class GetBookByIdQueryResponse
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = null!;
            public string Author { get; set; } = null!;
            public string Publisher { get; set; } = null!;
            public string Code { get; set; } = null!;
            public int PageCount { get; set; }
            public int StockCount { get; set; }
            public int PublishYear { get; set; }
            public string? Language { get; set; }
            public string? CoverImageUrl { get; set; }
            public string? Description { get; set; }
            public Guid CategoryId { get; set; }
        }

        public class GetBookByIdQueryRequest : IRequest<GetBookByIdQueryResponse?>
        {// ? kullanmamızın sebebi compailer hata vermesin null olabilir kontrolünde hata almayalım diye burası null gelebilir diyoruz.
            public Guid Id { get; set; }//requestte istenecek özellikler
        }

        public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQueryRequest, GetBookByIdQueryResponse?>
        {
            private readonly IBookRepository _bookRepository;

            public GetBookByIdQueryHandler(IBookRepository bookRepository)
            {
                _bookRepository = bookRepository;
            }
            public async Task<GetBookByIdQueryResponse?> Handle(GetBookByIdQueryRequest request, CancellationToken cancellationToken)
            {
                var book = await _bookRepository.GetByIdAsync(request.Id);
                if (book == null) throw new NotFoundException(ExceptionsResources.BookNotFound);

                //return book.Select(x => new GetBookByIdQueryResponse
                //birden fazla book listelenseydi yukardaki gibi yazacaktık

                return new GetBookByIdQueryResponse
                {//her book entitiy si response dönüştürülür. Buna mapping denir.
                    Id = book.Id,
                    Name = book.Name,
                    Author = book.Author,
                    Publisher = book.Publisher,
                    Code = book.Code,
                    PageCount = book.PageCount,
                    StockCount = book.StockCount,
                    PublishYear = book.PublishYear,
                    Language = book.Language,
                    CoverImageUrl = book.CoverImageUrl,
                    Description = book.Description,
                    CategoryId = book.CategoryId
                };
            }

        }



    }
}
