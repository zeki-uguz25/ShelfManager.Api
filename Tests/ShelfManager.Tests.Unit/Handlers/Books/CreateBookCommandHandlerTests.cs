using AutoMapper;
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
    public class CreateBookCommandHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepository = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IBookCacheService> _bookCacheService = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly CreateBookCommandHandler _handler;

        public CreateBookCommandHandlerTests()
        {
            _handler = new CreateBookCommandHandler(
                _bookRepository.Object,
                _unitOfWork.Object,
                _bookCacheService.Object,
                _mapper.Object);
        }


        [Fact]
        public async Task Handle_WhenRequestIsValid_ReturnsCreatedBookId()
        {
            // Arrange
            var book = new Book { Id = Guid.NewGuid() };

            _mapper.Setup(x => x.Map<Book>(It.IsAny<CreateBookCommandRequest>())).Returns(book);

            var request = new CreateBookCommandRequest
            {
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

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(book.Id);
            _bookRepository.Verify(x => x.AddAsync(It.IsAny<Book>()), Times.Once);
            _bookCacheService.Verify(x => x.InvalidateAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
