using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShelfManager.Application.Handlers.Categories.Commands
{
    public class DeleteCategoryCommandResponse
    {

    }

    public class DeleteCategoryCommandRequest : IRequest<DeleteCategoryCommandResponse>
    {
        public Guid Id { get; set; }
    }
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommandRequest, DeleteCategoryCommandResponse> 
    {
        private readonly ICategoryRepository _categoryRepository;
        
        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<DeleteCategoryCommandResponse> Handle(DeleteCategoryCommandRequest request, CancellationToken cancellationToken)
        {
            var category= await _categoryRepository.GetByIdAsync(request.Id);
            if (category == null)
            {
                throw new Exception("Kategori bulunamadı");
            }

            await _categoryRepository.DeleteAsync(category);
            return new DeleteCategoryCommandResponse { };
        }


    }
}
