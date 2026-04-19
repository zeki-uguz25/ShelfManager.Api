using AutoMapper;
using Core.Exception.Exceptions;
using Core.Exception.Resources;
using Core.Extensions;
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
        private readonly IMapper _mapper;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UpdateCategoryCommandResponse> Handle(UpdateCategoryCommandRequest request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            (category == null).IfTrueThrow(() => new NotFoundException(ExceptionsResources.CategoryNotFound));

            _mapper.Map(request, category); // request alanlarını mevcut entity üzerine yazar

            await _categoryRepository.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdateCategoryCommandResponse { Id = category.Id };
        }
    }
}
