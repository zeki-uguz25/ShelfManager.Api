using Core.Exception.Exceptions;
using Core.Exception.Resources;
using MediatR;
using ShelfManager.Application.Abstractions.Repositories;

namespace ShelfManager.Application.Handlers.Categories.Queries
{
    public class GetCategoryByIdQueryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class GetCategoryByIdQueryRequest : IRequest<GetCategoryByIdQueryResponse?>
    {
        public Guid Id { get; set; }
    }
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQueryRequest,  GetCategoryByIdQueryResponse?>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<GetCategoryByIdQueryResponse?> Handle(GetCategoryByIdQueryRequest request,CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            if (category == null)
            {
                throw new NotFoundException(ExceptionsResources.CategoryNotFound);
            }
            return new GetCategoryByIdQueryResponse
            {
                Id = category.Id,
                Name = category.Name,
            };

        }
    }
}
