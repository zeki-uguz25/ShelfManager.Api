using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShelfManager.Application.Handlers.Books.Queries;

public class GetAllBooksQuery
{
    public class GetAllBooksQueryResponse
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
    //Entity'yi direkt döndürmüyoruz — sadece lazım olan alanları döndürüyoruz
    //Buna DTO(Data Transfer Object) denir

    public class GetAllBooksQueryRequest : IRequest<IEnumerable<GetAllBooksQueryResponse>>
    {    //IRequest<...> — "cevap olarak bu tipi bekliyorum" demek

    }

    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQueryRequest, IEnumerable<GetAllBooksQueryResponse>>
    {//"bu request gelince ben karşılarım" demek
        private readonly IBookRepository _bookRepository;

        public GetAllBooksQueryHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<GetAllBooksQueryResponse>> Handle(GetAllBooksQueryRequest request, CancellationToken cancellationToken)
        {//response da bir liste döndüğü için response geçen her yerde IEnumerable kullandık
            var books = await _bookRepository.GetAllAsync();//tüm kitapları db den çeker.

            return books.Select(x => new GetAllBooksQueryResponse //Response da birden fazla dğer döndüğü için yani bir liste burda select kullandık.
            {//her book entitiy si response dönüştürülür. Buna mapping denir.
                Id = x.Id,
                Name = x.Name,
                Author = x.Author,
                Publisher = x.Publisher,
                Code = x.Code,
                PageCount = x.PageCount,
                StockCount = x.StockCount,
                PublishYear = x.PublishYear,
                Language = x.Language,
                CoverImageUrl = x.CoverImageUrl,
                Description = x.Description,
                CategoryId = x.CategoryId
            });
        }
    }
}
