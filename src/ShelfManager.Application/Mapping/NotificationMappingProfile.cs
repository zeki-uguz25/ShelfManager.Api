using AutoMapper;
using ShelfManager.Application.Handlers.Notifications.Queries;
using ShelfManager.Domain.Entities;

namespace ShelfManager.Application.Mapping;

public class NotificationMappingProfile : Profile
{
    public NotificationMappingProfile()
    {
        // Entity → Response (okuma işlemleri)
        CreateMap<Notification, GetUserNotificationsQueryResponse>();
    }
}
