using AutoMapper;
using Core.Exception.Exceptions;
using Core.Exception.Resources;
using Core.Extensions;
using MediatR;
using ShelfManager.Application.Abstractions.Repositories;

namespace ShelfManager.Application.Handlers.Books.Queries
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
            private readonly IMapper _mapper;

            public GetBookByIdQueryHandler(IBookRepository bookRepository, IMapper mapper)
            {
                _bookRepository = bookRepository;
                _mapper = mapper;
            }
            public async Task<GetBookByIdQueryResponse?> Handle(GetBookByIdQueryRequest request, CancellationToken cancellationToken)
            {
                var book = await _bookRepository.GetByIdAsync(request.Id);
                (book == null).IfTrueThrow(() => new NotFoundException(ExceptionsResources.BookNotFound));

                //return book.Select(x => new GetBookByIdQueryResponse
                //birden fazla book listelenseydi yukardaki gibi yazacaktık

                return _mapper.Map<GetBookByIdQueryResponse>(book); //her book entity si response a dönüştürülür. Buna mapping denir.
            }

        }



    
}
