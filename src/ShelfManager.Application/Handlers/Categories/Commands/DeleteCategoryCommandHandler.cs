using Core.Exception.Exceptions;
using Core.Exception.Resources;
using Core.Persistence.EntityFrameworkCore.UnitOfWork;
using MediatR;
using ShelfManager.Application.Abstractions.Repositories;

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
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteCategoryCommandResponse> Handle(DeleteCategoryCommandRequest request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            if (category == null)
                throw new NotFoundException(ExceptionsResources.CategoryNotFound);

            await _categoryRepository.DeleteAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new DeleteCategoryCommandResponse { };
        }
    }
}
