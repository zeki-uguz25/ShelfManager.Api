using AutoMapper;
using Core.Persistence.EntityFrameworkCore.Pagination;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Application.Constants;
using System.Text.Json;

namespace ShelfManager.Application.Handlers.Books.Queries;

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

    public class GetAllBooksQueryRequest : PagedRequest, IRequest<PagedList<GetAllBooksQueryResponse>>
    {
    }

    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQueryRequest, PagedList<GetAllBooksQueryResponse>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IDistributedCache _cache;
        private readonly IMapper _mapper;

        public GetAllBooksQueryHandler(IBookRepository bookRepository, IDistributedCache cache, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<PagedList<GetAllBooksQueryResponse>> Handle(GetAllBooksQueryRequest request, CancellationToken cancellationToken)
        {
            var cacheKey = CacheKeys.Books(request.PageNumber, request.PageSize);

            var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);//rediste arar yoksa cached null olur
            if (cached != null)
                return JsonSerializer.Deserialize<PagedList<GetAllBooksQueryResponse>>(cached)!;

            var pagedBooks = await _bookRepository.GetPagedAsync(request.PageNumber, request.PageSize);

            var result = new PagedList<GetAllBooksQueryResponse>
            {
                Items = _mapper.Map<List<GetAllBooksQueryResponse>>(pagedBooks.Items),
                TotalCount = pagedBooks.TotalCount,
                PageNumber = pagedBooks.PageNumber,
                PageSize = pagedBooks.PageSize
            };

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                },
                cancellationToken);//redise ekleme yapar

            return result;
        }
    }
