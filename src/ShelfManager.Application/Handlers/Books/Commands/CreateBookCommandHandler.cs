using AutoMapper;
using Core.Persistence.EntityFrameworkCore.UnitOfWork;
using FluentValidation;
using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Application.Abstractions.Services;
using ShelfManager.Application.Handlers.Books.Resources;
using ShelfManager.Domain.Entities;

namespace ShelfManager.Application.Handlers.Books.Commands
{

    

        public class CreateBookCommandResponse
        {
            public Guid Id { get; set; }
        }

        public class CreateBookCommandRequest : IRequest<CreateBookCommandResponse>
        {
            public string Name { get; set; } = null!;
            public string? Description { get; set; }
            public int PageCount { get; set; }
            public string Author { get; set; } = null!;
            public int StockCount { get; set; }
            public int TotalCount { get; set; }
            public int PublishYear { get; set; }
            public string Publisher { get; set; } = null!;
            public string Code { get; set; } = null!;
            public string? Language { get; set; }
            public Guid CategoryId { get; set; }
            public string? CoverImageUrl { get; set; }
        }

        public class CreateBookCommandValidator : AbstractValidator<CreateBookCommandRequest>
        {
            public CreateBookCommandValidator()
            {
                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage(_ => ValidationMessages.Name_Required)
                    .MaximumLength(200).WithMessage(_ => ValidationMessages.Name_MaxLength);

                RuleFor(x => x.Description)
                    .MaximumLength(2000).WithMessage(_ => ValidationMessages.Description_MaxLength);

                RuleFor(x => x.Author)
                    .NotEmpty().WithMessage(_ => ValidationMessages.Author_Required)
                    .MaximumLength(200).WithMessage(_ => ValidationMessages.Author_MaxLength);

                RuleFor(x => x.PageCount)
                    .GreaterThan(0).WithMessage(_ => ValidationMessages.PageCount_GreaterThanZero);

                RuleFor(x => x.StockCount)
                    .GreaterThanOrEqualTo(0).WithMessage(_ => ValidationMessages.StockCount_GreaterThanOrEqualZero);

                RuleFor(x => x.TotalCount)
                    .GreaterThan(0).WithMessage(_ => ValidationMessages.TotalCount_GreaterThanZero);

                RuleFor(x => x.PublishYear)
                    .GreaterThan(0).WithMessage(_ => ValidationMessages.PublishYear_Required);

                RuleFor(x => x.Publisher)
                    .NotEmpty().WithMessage(_ => ValidationMessages.Publisher_Required);

                RuleFor(x => x.Code)
                    .NotEmpty().WithMessage(_ => ValidationMessages.Code_Required);

                RuleFor(x => x.CategoryId)
                    .NotEmpty().WithMessage(_ => ValidationMessages.CategoryId_Required);
            }
        }
        public class CreateBookCommandHandler : IRequestHandler<CreateBookCommandRequest, CreateBookCommandResponse>
        {
            private readonly IBookRepository _bookRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IBookCacheService _bookCacheService;
            private readonly IMapper _mapper;

            public CreateBookCommandHandler(IBookRepository bookRepository, IUnitOfWork unitOfWork, IBookCacheService bookCacheService, IMapper mapper)
            {
                _bookRepository = bookRepository;
                _unitOfWork = unitOfWork;
                _bookCacheService = bookCacheService;
                _mapper = mapper;
            }

            public async Task<CreateBookCommandResponse> Handle(CreateBookCommandRequest request, CancellationToken cancellationToken)
            {
                var book = _mapper.Map<Book>(request); // request → entity mapping
                book.Id = Guid.NewGuid(); //Tüm itemler doldurulmalı.
                book.CreatedAt = DateTime.UtcNow;

                await _bookRepository.AddAsync(book);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _bookCacheService.InvalidateAsync(cancellationToken);

                return new CreateBookCommandResponse { Id = book.Id };
            }
        }
    
}
