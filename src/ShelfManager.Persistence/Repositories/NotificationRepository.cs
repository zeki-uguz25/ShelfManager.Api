using Core.Persistence.EntityFrameworkCore;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Domain.Entities;
using ShelfManager.Persistence.Context;

namespace ShelfManager.Persistence.Repositories;

public class NotificationRepository : EFEntityBaseRepository<Notification, ShelfManagerDbContext>, INotificationRepository
{
    public NotificationRepository(ShelfManagerDbContext context) : base(context)
    {
    }
}
