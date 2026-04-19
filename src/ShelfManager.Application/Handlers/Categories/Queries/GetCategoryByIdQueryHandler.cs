using AutoMapper;
using Core.Exception.Exceptions;
using Core.Exception.Resources;
using Core.Extensions;
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
        private readonly IMapper _mapper;

        public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<GetCategoryByIdQueryResponse?> Handle(GetCategoryByIdQueryRequest request,CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            (category == null).IfTrueThrow(() => new NotFoundException(ExceptionsResources.CategoryNotFound));
            return _mapper.Map<GetCategoryByIdQueryResponse>(category);
        }
    }
}
