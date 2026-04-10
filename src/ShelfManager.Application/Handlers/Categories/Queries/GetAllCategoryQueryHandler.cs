using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShelfManager.Application.Handlers.Categories.Queries
{
    public class GetAllCategoryQueryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
    public class GetAllCategoryQueryRequest : IRequest<IEnumerable<GetAllCategoryQueryResponse>> 
    {
        //Response liste olduğu için tüm responselerden önce IEnumerable ekledik
    }
    public class GetAllCategoryQueryHandler : IRequestHandler<GetAllCategoryQueryRequest, IEnumerable<GetAllCategoryQueryResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetAllCategoryQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable< GetAllCategoryQueryResponse>> Handle(GetAllCategoryQueryRequest request,CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAllAsync();

            return categories.Select(x => new GetAllCategoryQueryResponse
            {
                Id = x.Id,
                Name = x.Name
            });
        }

    }
}
