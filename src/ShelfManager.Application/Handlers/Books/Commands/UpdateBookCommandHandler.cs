using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommandRequest, UpdateBookCommandResponse>
        {
            private readonly IBookRepository _bookRepository;

            public UpdateBookCommandHandler(IBookRepository bookRepository)
            {
                _bookRepository = bookRepository;
            }

            public async Task<UpdateBookCommandResponse> Handle(UpdateBookCommandRequest request, CancellationToken cancellationToken)
            {
                var book = await _bookRepository.GetByIdAsync(request.Id);//requestten gelen Id yi veritabanın da radık ve veri tabanında bulunan kaydı book variable sine ekledik
                if (book == null) throw new Exception("Kitap bulunamadı.");

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

                return new UpdateBookCommandResponse { Id = book.Id };
            }

        }
    }
}
