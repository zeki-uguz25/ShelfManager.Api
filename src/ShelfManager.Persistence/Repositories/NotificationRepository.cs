using Core.Persistence.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Domain.Entities;
using ShelfManager.Persistence.Context;

namespace ShelfManager.Persistence.Repositories;

public class NotificationRepository : EFEntityBaseRepository<Notification, ShelfManagerDbContext>, INotificationRepository
{
    public NotificationRepository(ShelfManagerDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Notification>> GetAllByUserIdAsync(Guid userId)
    {
        return await _context.Notifications
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public Task SoftDeleteAsync(Notification notification)
    {
        notification.IsDeleted = true;
        notification.DeletedAt = DateTime.UtcNow;
        _context.Notifications.Update(notification);
        return Task.CompletedTask;
    }
}
