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
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ShelfManager.Tests.Unit.Handlers.Books
{
    public class DeleteBookCommandHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepository =new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IBookCacheService> _bookCacheService = new();
        private readonly DeleteBookCommandHandler _handler;


        public DeleteBookCommandHandlerTests()
        {
            _handler = new DeleteBookCommandHandler(
                    _bookRepository.Object,
                    _unitOfWork.Object,
                    _bookCacheService.Object);
        }
        [Fact]
        public async Task Handle_WhenBookNotFound_ThrowsNotFoundException()
        {
            _bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Book?)null);

            var request = new DeleteBookCommandRequest { Id = Guid.NewGuid() };

            var act = async () => await _handler.Handle(request, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task Handle_WhenRequestIsValid_DeletesBook()
        {
            var book = new Book { Id = Guid.NewGuid() };

            _bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(book);

            var request = new DeleteBookCommandRequest { Id = Guid.NewGuid() };

            var result = await _handler.Handle(request, CancellationToken.None);

            result.Should().NotBeNull();
            _bookRepository.Verify(x => x.DeleteAsync(It.Is<Book>(b => b.Id == book.Id)), Times.Once);
            _bookCacheService.Verify(x => x.InvalidateAsync(It.IsAny<CancellationToken>()), Times.Once);

        }

    }
}
