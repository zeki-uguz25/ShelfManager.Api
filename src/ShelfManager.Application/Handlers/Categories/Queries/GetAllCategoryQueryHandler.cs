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

        public GetAllCategoryQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<PagedList<GetAllCategoryQueryResponse>> Handle(GetAllCategoryQueryRequest request, CancellationToken cancellationToken)
        {
            var paged = await _categoryRepository.GetPagedAsync(request.PageNumber, request.PageSize);

            return new PagedList<GetAllCategoryQueryResponse>
            {
                Items = paged.Items.Select(x => new GetAllCategoryQueryResponse
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList(),
                TotalCount = paged.TotalCount,
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize
            };
        }
    }
}
