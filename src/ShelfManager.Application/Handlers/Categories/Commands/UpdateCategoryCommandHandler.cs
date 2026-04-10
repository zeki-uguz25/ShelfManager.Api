using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShelfManager.Application.Handlers.Categories.Commands
{
    public class UpdateCategoryCommandResponse
    {
        public Guid Id { get; set; }
    }

    public class UpdateCategoryCommandRequest : IRequest<UpdateCategoryCommandResponse>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommandRequest, UpdateCategoryCommandResponse> 
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<UpdateCategoryCommandResponse> Handle(UpdateCategoryCommandRequest request,CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            if(category == null)
            {
                throw new Exception("Kategori bulunamadı");
            }

            category.Name = request.Name;

            await _categoryRepository.UpdateAsync(category);

            return new UpdateCategoryCommandResponse { Id = category.Id };
        }
    }
}
