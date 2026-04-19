using AutoMapper;
using Core.Persistence.EntityFrameworkCore.Pagination;
using MediatR;
using ShelfManager.Application.Abstractions.Repositories;

namespace ShelfManager.Application.Handlers.Categories.Queries
{
    public class GetAllCategoryQueryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class GetAllCategoryQueryRequest : PagedRequest, IRequest<PagedList<GetAllCategoryQueryResponse>>
    {
    }

    public class GetAllCategoryQueryHandler : IRequestHandler<GetAllCategoryQueryRequest, PagedList<GetAllCategoryQueryResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GetAllCategoryQueryHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<PagedList<GetAllCategoryQueryResponse>> Handle(GetAllCategoryQueryRequest request, CancellationToken cancellationToken)
        {
            var paged = await _categoryRepository.GetPagedAsync(request.PageNumber, request.PageSize);

            return new PagedList<GetAllCategoryQueryResponse>
            {
                Items = _mapper.Map<List<GetAllCategoryQueryResponse>>(paged.Items),
                TotalCount = paged.TotalCount,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize
            };
        }
    }
}
