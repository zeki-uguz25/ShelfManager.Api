using Core.Exception.Exceptions;
using Core.Exception.Resources;
using Core.Persistence.EntityFrameworkCore.UnitOfWork;
using FluentValidation;
using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Application.Handlers.Categories.Resources;

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
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommandRequest>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(_ => ValidationMessages.Name_Required)
                .MaximumLength(100).WithMessage(_ => ValidationMessages.Name_MaxLength);
        }
    }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommandRequest, UpdateCategoryCommandResponse>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateCategoryCommandResponse> Handle(UpdateCategoryCommandRequest request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            if (category == null)
                throw new NotFoundException(ExceptionsResources.CategoryNotFound);

            category.Name = request.Name;

            await _categoryRepository.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdateCategoryCommandResponse { Id = category.Id };
        }
    }
}
