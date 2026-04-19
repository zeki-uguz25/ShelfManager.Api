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
using static ShelfManager.Domain.Constants.Permissions;

namespace ShelfManager.Tests.Unit.Handlers.Books
{
    public class BorrowBookCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly Mock<IBookRepository> _bookRepository = new();
        private readonly Mock<IUserBookRepository> _userBookRepository = new();
        private readonly Mock<IAuthService> _authService = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IBookCacheService> _bookCacheService = new();
        private readonly BorrowBookCommandHandler _handler;

        public BorrowBookCommandHandlerTests()
        {
            _handler = new BorrowBookCommandHandler(
                    _userBookRepository.Object,
                    _bookRepository.Object,
                    _userRepository.Object,
                    _authService.Object,
                    _unitOfWork.Object,
                    _bookCacheService.Object);
        }

        [Fact]
        public async Task Handle_WhenBookNotFound_ThrowsNotFoundException()
        {
            // Arrange
            _bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Book?)null);
            _authService.Setup(x => x.GetCurrentUserId()).Returns(Guid.NewGuid());

            var request = new BorrowBookCommandRequest { BookId = Guid.NewGuid() };

            // Act
            var act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task Handle_WhenBookOutOfStock_ThrowsBusinessException()
        {
            // Arrange
            _bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Book { Id = Guid.NewGuid(), StockCount = 0 });
            _authService.Setup(x => x.GetCurrentUserId()).Returns(Guid.NewGuid());//şu metot çağırıldığında bunu döndür

            var request = new BorrowBookCommandRequest { BookId = Guid.NewGuid() };

            // Act
            var act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<BusinessException>();
        }

        [Fact]
        public async Task Handle_WhenUserIsBanned_ThrowsBusinessException()
        {
            var user = new User { Id = Guid.NewGuid(), IsBanned = true };
            var book = new Book { Id = Guid.NewGuid(), StockCount = 5 };

            _authService.Setup(x => x.GetCurrentUserId()).Returns(user.Id);
            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(book);

            var request = new BorrowBookCommandRequest { BookId = book.Id };

            var act = async () => await _handler.Handle(request, CancellationToken.None);

            await act.Should().ThrowAsync<BusinessException>();


        }

        [Fact]
        public async Task Handle_WhenUserReachedBorrowLimit_ThrowsBusinessException()
        {
            // Arrange
            var book = new Book { StockCount = 5 };
            var user = new User { IsBanned = false };
            var userBooks = new List<UserBook>
    {
        new UserBook(),
        new UserBook(),
        new UserBook()
    };

            _authService.Setup(x => x.GetCurrentUserId()).Returns(Guid.NewGuid());
            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(book);
            _userBookRepository.Setup(x => x.GetBorrowedBookCountByUserIdAsync(It.IsAny<Guid>())).ReturnsAsync(userBooks);

            var request = new BorrowBookCommandRequest { BookId = Guid.NewGuid() };

            // Act
            var act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<BusinessException>();
        }

        [Fact]
        public async Task Handle_WhenUserAlreadyBorrowedBook_ThrowsBusinessException()
        {
            // Arrange
            var book = new Book { Id = Guid.NewGuid(), StockCount = 1 };
            var user = new User { IsBanned = false };
            var userBooks = new List<UserBook> { new UserBook { BookId = book.Id } };

            _authService.Setup(x => x.GetCurrentUserId()).Returns(Guid.NewGuid());
            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(book);
            _userBookRepository.Setup(x => x.GetBorrowedBookCountByUserIdAsync(It.IsAny<Guid>())).ReturnsAsync(userBooks);

            var request = new BorrowBookCommandRequest { BookId = book.Id };

            // Act
            var act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<BusinessException>();
        }


        [Fact]
        public async Task Handle_WhenRequestIsValid_ReturnsBorrowBookResponse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { IsBanned = false, FullName = "Test User" };
            var book = new Book { Id = Guid.NewGuid(), StockCount = 5, Name = "Clean Code" };

            _authService.Setup(x => x.GetCurrentUserId()).Returns(userId);
            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _bookRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(book);
            _userBookRepository.Setup(x => x.GetBorrowedBookCountByUserIdAsync(It.IsAny<Guid>())).ReturnsAsync(new List<UserBook>());

            var request = new BorrowBookCommandRequest { BookId = book.Id };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.BookName.Should().Be(book.Name);
            _bookRepository.Verify(x => x.UpdateAsync(It.Is<Book>(b => b.StockCount == 4)), Times.Once);
            _userBookRepository.Verify(x => x.AddAsync(It.Is<UserBook>(ub => ub.BookId == book.Id)), Times.Once);
        }










    }
}
