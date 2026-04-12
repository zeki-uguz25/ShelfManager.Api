using Core.Exception.Exceptions;
using Core.Exception.Resources;
using Core.Persistence.EntityFrameworkCore.UnitOfWork;
using FluentValidation;
using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Application.Abstractions.Services;
using ShelfManager.Application.Handlers.Books.Resources;

namespace ShelfManager.Application.Handlers.Books.Commands
{

    public class UpdateBookCommand
    {
        public class UpdateBookCommandResponse
        {
            public Guid Id { get; set; }

        }

        public class UpdateBookCommandRequest : IRequest<UpdateBookCommandResponse>
        {
            public Guid Id { get; set; }
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
        public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommandRequest>
        {
            public UpdateBookCommandValidator()
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

        public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommandRequest, UpdateBookCommandResponse>
        {
            private readonly IBookRepository _bookRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IBookCacheService _bookCacheService;

            public UpdateBookCommandHandler(IBookRepository bookRepository, IUnitOfWork unitOfWork, IBookCacheService bookCacheService)
            {
                _bookRepository = bookRepository;
                _unitOfWork = unitOfWork;
                _bookCacheService = bookCacheService;
            }

            public async Task<UpdateBookCommandResponse> Handle(UpdateBookCommandRequest request, CancellationToken cancellationToken)
            {
                var book = await _bookRepository.GetByIdAsync(request.Id);
                if (book == null) throw new NotFoundException(ExceptionsResources.BookNotFound);

                book.Name = request.Name;
                book.Description = request.Description;
                book.PageCount = request.PageCount;
                book.Author = request.Author;
                book.StockCount = request.StockCount;
                book.TotalCount = request.TotalCount;
                book.PublishYear = request.PublishYear;
                book.Publisher = request.Publisher;
                book.Code = request.Code;
                book.Language = request.Language;
                book.CategoryId = request.CategoryId;
                book.CoverImageUrl = request.CoverImageUrl;

                await _bookRepository.UpdateAsync(book);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _bookCacheService.InvalidateAsync(cancellationToken);

                return new UpdateBookCommandResponse { Id = book.Id };
            }
        }
    }
}
