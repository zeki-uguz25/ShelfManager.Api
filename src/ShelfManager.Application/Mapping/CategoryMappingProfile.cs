using AutoMapper;
using ShelfManager.Application.Handlers.Categories.Commands;
using ShelfManager.Application.Handlers.Categories.Queries;
using ShelfManager.Domain.Entities;

namespace ShelfManager.Application.Mapping;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        // Entity → Response (okuma işlemleri)
        CreateMap<Category, GetAllCategoryQueryResponse>();
        CreateMap<Category, GetCategoryByIdQueryResponse>();

        // Request → Entity (yazma işlemleri)
        CreateMap<CreateCategoryCommandRequest, Category>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())        // Id handler'da set edilir
            .ForMember(dest => dest.Books, opt => opt.Ignore());    // navigation property

        CreateMap<UpdateCategoryCommandRequest, Category>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())        // Id URL'den gelir
            .ForMember(dest => dest.Books, opt => opt.Ignore());    // navigation property
    }
}
