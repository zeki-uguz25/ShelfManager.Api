using MediatR;
using ShelfManager.Application.Abstractions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShelfManager.Application.Handlers.Books.Commands
{
    public class DeleteBookCommand
    {
        public class DeleteBookCommandResponse
        {
        }
        public class DeleteBookCommandRequest : IRequest<DeleteBookCommandResponse>
        {
            public Guid Id {  get; set; }
        }



        public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommandRequest, DeleteBookCommandResponse>
        {
            private readonly IBookRepository _bookRepository;

            public DeleteBookCommandHandler(IBookRepository bookRepository)
            {
                _bookRepository= bookRepository;
            }

            public async Task<DeleteBookCommandResponse> Handle(DeleteBookCommandRequest request, CancellationToken cancellationToken)
            {
                var book = await _bookRepository.GetByIdAsync(request.Id);
                if (book == null) throw new Exception("Kitap bulunamadı.");

                await _bookRepository.DeleteAsync(book);

                return new DeleteBookCommandResponse { };


            }
        }
    }
}
