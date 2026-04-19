using AutoMapper;
using Core.Exception.Exceptions;
using Core.Persistence.EntityFrameworkCore.UnitOfWork;
using FluentAssertions;
using Moq;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Application.Abstractions.Services;
using ShelfManager.Application.Handlers.Books.Commands;
using ShelfManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShelfManager.Tests.Unit.Handlers.Books
{
    public class UpdateBookCommandHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepository = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IBookCacheService> _bookCacheService = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly UpdateBookCommandHandler _handler;
        public UpdateBookCommandHandlerTests()
        {

            _handler = new UpdateBookCommandHandler(
                _bookRepository.Object,
                _unitOfWork.Object,
                _bookCacheService.Object,
                _mapper.Object
                );
        }

        [Fact]
        public async Task Handle_WhenBookNotFound_ThrowsNotFoundException()
        {

            _bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Book?)null);

            var request = new UpdateBookCommandRequest
            {
                Id= Guid.NewGuid(),
                Name = "Clean Code",
                Author = "Robert C. Martin",
                Publisher = "Prentice Hall",
                Code = "CC-001",
                PageCount = 464,
                StockCount = 5,
                TotalCount = 10,
                PublishYear = 2008,
                CategoryId = Guid.NewGuid()
            };

            var act = async () => await _handler.Handle(request, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>();

        }

        [Fact]
        public async Task Handle_WhenRequestIsValid_UpdatesBook()
        {
            var book = new Book { Id = Guid.NewGuid() };

            _bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(book);
            _mapper.Setup(x => x.Map(It.IsAny<UpdateBookCommandRequest>(), It.IsAny<Book>()));

            var request = new UpdateBookCommandRequest
            {
                Id = book.Id,
                Name = "Clean Code",
                Author = "Robert C. Martin",
                Publisher = "Prentice Hall",
                Code = "CC-001",
                PageCount = 464,
                StockCount = 5,
                TotalCount = 10,
                PublishYear = 2008,
                CategoryId = Guid.NewGuid()
            };

            var result = await _handler.Handle(request, CancellationToken.None);

            result.Should().NotBeNull();
            _bookRepository.Verify(x => x.UpdateAsync(It.IsAny<Book>()), Times.Once);
            _bookCacheService.Verify(x => x.InvalidateAsync(It.IsAny<CancellationToken>()), Times.Once);
        }


    }
}
