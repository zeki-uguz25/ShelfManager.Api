using Core.Exception.Exceptions;
using Core.Exception.Resources;
using Core.Persistence.EntityFrameworkCore.UnitOfWork;
using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Application.Abstractions.Services;

namespace ShelfManager.Application.Handlers.Books.Commands
{
    public class DeleteBookCommand
    {
        public class DeleteBookCommandResponse
        {
        }

        public class DeleteBookCommandRequest : IRequest<DeleteBookCommandResponse>
        {
            public Guid Id { get; set; }
        }

        public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommandRequest, DeleteBookCommandResponse>
        {
            private readonly IBookRepository _bookRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IBookCacheService _bookCacheService;

            public DeleteBookCommandHandler(IBookRepository bookRepository, IUnitOfWork unitOfWork, IBookCacheService bookCacheService)
            {
                _bookRepository = bookRepository;
                _unitOfWork = unitOfWork;
                _bookCacheService = bookCacheService;
            }

            public async Task<DeleteBookCommandResponse> Handle(DeleteBookCommandRequest request, CancellationToken cancellationToken)
            {
                var book = await _bookRepository.GetByIdAsync(request.Id);
                if (book == null) throw new NotFoundException(ExceptionsResources.BookNotFound);

                await _bookRepository.DeleteAsync(book);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _bookCacheService.InvalidateAsync(cancellationToken);

                return new DeleteBookCommandResponse { };
            }
        }
    }
}
