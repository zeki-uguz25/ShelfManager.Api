using AutoMapper;
using ShelfManager.Application.Handlers.Users.Queries;
using ShelfManager.Domain.Entities;

namespace ShelfManager.Application.Mapping;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        // Entity → Response (okuma işlemleri)
        CreateMap<User, GetAllUsersQueryResponse>();
        CreateMap<User, GetUserByIdQueryResponse>();
    }
}
