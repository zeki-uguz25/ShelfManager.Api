using AutoMapper;
using ShelfManager.Application.Handlers.Books.Commands;
using ShelfManager.Application.Handlers.Books.Queries;
using ShelfManager.Domain.Entities;

namespace ShelfManager.Application.Mapping;

public class BookMappingProfile : Profile
{
    public BookMappingProfile()
    {
        // Entity → Response (okuma işlemleri)
        CreateMap<Book, GetAllBooksQueryResponse>();
        CreateMap<Book, GetBookByIdQueryResponse>();

        // Request → Entity (yazma işlemleri)
        CreateMap<CreateBookCommandRequest, Book>()// Requestte olmayanlar ve elle girilecekler ignore edilir.
            .ForMember(dest => dest.Id, opt => opt.Ignore())           // Id handler'da set edilir
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())    // CreatedAt handler'da set edilir
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())    // soft delete alanı
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())    // soft delete alanı
            .ForMember(dest => dest.Category, opt => opt.Ignore())     // navigation property
            .ForMember(dest => dest.UserBooks, opt => opt.Ignore());   // navigation property

        CreateMap<UpdateBookCommandRequest, Book>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())           // Id URL'den gelir
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())    // değişmemeli
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())    // soft delete alanı
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())    // soft delete alanı
            .ForMember(dest => dest.Category, opt => opt.Ignore())     // navigation property
            .ForMember(dest => dest.UserBooks, opt => opt.Ignore());   // navigation property
    }
}
