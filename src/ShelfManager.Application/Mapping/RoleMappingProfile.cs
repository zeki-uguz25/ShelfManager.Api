using AutoMapper;
using ShelfManager.Application.Handlers.Roles.Queries;
using ShelfManager.Domain.Entities;

namespace ShelfManager.Application.Mapping;

public class RoleMappingProfile : Profile
{
    public RoleMappingProfile()
    {
        // Entity → Response (okuma işlemleri)
        CreateMap<Role, GetAllRolesQueryResponse>();
    }
}
