using AutoMapper;
using ShelfManager.Application.Handlers.Fines.Queries;
using ShelfManager.Domain.Entities;

namespace ShelfManager.Application.Mapping;

public class FineMappingProfile : Profile
{
    public FineMappingProfile()
    {
        // Entity → Response (okuma işlemleri)
        CreateMap<Fine, GetUserFinesQueryResponse>();
    }
}
