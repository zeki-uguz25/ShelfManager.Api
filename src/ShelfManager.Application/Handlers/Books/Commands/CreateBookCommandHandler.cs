using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShelfManager.Application.Handlers.Books.Commands
{

    public class CreateBookCommand
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
        public class CreateBookCommandHandler : IRequestHandler<CreateBookCommandRequest, CreateBookCommandResponse>
        {
            private readonly IBookRepository _bookRepository;

            public CreateBookCommandHandler(IBookRepository bookRepository)
            {
                _bookRepository=bookRepository;
            }

            public async Task<CreateBookCommandResponse> Handle(CreateBookCommandRequest request, CancellationToken cancellationToken)
            {
                var book = new Book
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description,
                    PageCount = request.PageCount,
                    Author = request.Author,
                    StockCount = request.StockCount,
                    TotalCount = request.TotalCount,
                    PublishYear = request.PublishYear,
                    Publisher=request.Publisher,
                    Code=request.Code,
                    Language = request.Language,
                    CategoryId = request.CategoryId,
                    CoverImageUrl = request.CoverImageUrl
                    
                };

                await _bookRepository.AddAsync(book);

                return new CreateBookCommandResponse { Id = book.Id };

            }
        }
    }
}
