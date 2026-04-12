using Core.Exception.Exceptions;
using Core.Exception.Resources;
using Core.Persistence.EntityFrameworkCore.UnitOfWork;
using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShelfManager.Application.Handlers.Books.Queries
{
    public class GetBooksByCategoryQueryResponse
    {
        public Guid BookId { get; set; }
        public string BookName { get; set; }
    }

    public class GetBooksByCategoryQueryRequest : IRequest<IEnumerable<GetBooksByCategoryQueryResponse>>
    {
        public Guid CategoryId { get; set; }
    }
    public class GetBooksByCategoryQueryHandler :IRequestHandler<GetBooksByCategoryQueryRequest,IEnumerable<GetBooksByCategoryQueryResponse>> 
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GetBooksByCategoryQueryHandler(IBookRepository bookRepository, ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<GetBooksByCategoryQueryResponse>> Handle(GetBooksByCategoryQueryRequest request,CancellationToken cancellationToken)
        {//kategori id verildiðinde kitaplar listelenecek
            var category= await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (category == null)
            {
                throw new NotFoundException(ExceptionsResources.CategoryNotFound);
            }

            var books = await _bookRepository.GetBooksByCategory(category.Id);

            return books.Select(book => new GetBooksByCategoryQueryResponse
            {
                BookId = book.Id,
                BookName = book.Name
            });

        }
    }
}
