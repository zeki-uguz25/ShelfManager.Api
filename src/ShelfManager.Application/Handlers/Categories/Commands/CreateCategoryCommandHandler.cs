using AutoMapper;
using Core.Persistence.EntityFrameworkCore.UnitOfWork;
using FluentValidation;
using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Application.Handlers.Categories.Resources;
using ShelfManager.Domain.Entities;

namespace ShelfManager.Application.Handlers.Categories.Commands
{
    public class CreateCategoryCommandResponse
    {
        public Guid Id { get; set; }
    }

    public class CreateCategoryCommandRequest : IRequest<CreateCategoryCommandResponse>
    {
        public string Name { get; set; } = null!;
    }

    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommandRequest>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(_ => ValidationMessages.Name_Required)
                .MaximumLength(100).WithMessage(_ => ValidationMessages.Name_MaxLength);
        }
    }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommandRequest, CreateCategoryCommandResponse>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateCategoryCommandResponse> Handle(CreateCategoryCommandRequest request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Category>(request); // request → entity mapping
            category.Id = Guid.NewGuid();

            await _categoryRepository.AddAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateCategoryCommandResponse { Id = category.Id };
        }
    }
}
